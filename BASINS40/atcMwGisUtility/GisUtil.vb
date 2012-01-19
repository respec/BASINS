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

    Public Shared Function MappingObjectSet() As Boolean
        Return pMapWin IsNot Nothing
    End Function

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

            If aLayerIndex >= 0 AndAlso aLayerIndex < GetMappingObject.Layers.NumLayers Then
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
        '5/26/09 apparent coding error corrected by LCW
        'Dim lLineShape As MapWinGIS.Shape = lLineSf.Shape(aLineIndex - 1)
        Dim lLineShape As MapWinGIS.Shape = lLineSf.Shape(aLineIndex)
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
        'If lField.Type = MapWinGIS.FieldType.STRING_FIELD Then lField.Width = aFieldWidth
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

    ''' <summary>
    ''' Given a layer index, return the first associated group it is located in
    ''' </summary>
    ''' <param name="aLayerIndex">Index of layer</param>
    ''' <returns>Name of group or empty string if cannot be found</returns>
    ''' <remarks>Added by LCW 4/9/09 for GBMM project</remarks>
    Public Shared Function LayerGroup(ByVal aLayerIndex As Integer) As String
        Dim lyr As MapWindow.Interfaces.Layer = LayerFromIndex(aLayerIndex)
        With GetMappingObject.Layers
            For i As Integer = 0 To .Groups.Count - 1
                If .Groups(i).Handle = lyr.GroupHandle Then Return .Groups(i).Text
            Next
            Return ""
        End With
    End Function

    ''' <summary>
    ''' Given a layer name, return the associated group it is located in
    ''' </summary>
    ''' <param name="aLayerName">Name of layer</param>
    ''' <returns>Name of group or empty string if cannot be found</returns>
    ''' <remarks>Added by LCW 4/9/09 for GBMM project</remarks>
    Public Shared Function LayerGroup(ByVal aLayerName As String) As String
        Return LayerGroup(LayerIndex(aLayerName))
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

    ''' <summary>
    ''' Add group if it doesn't already exist; return group number
    ''' </summary>
    ''' <param name="aGroupName"></param>
    ''' <returns></returns>
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

    Public Shared Function AddLine(ByVal aLayerIndex As Integer, ByVal aX() As Double, ByVal aY() As Double) As Boolean
        Dim lResult As Boolean
        Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)

        Dim lLine As New MapWinGIS.Shape
        lResult = lLine.Create(MapWinGIS.ShpfileType.SHP_POLYLINE)

        For lPointIndex As Integer = 0 To aX.GetUpperBound(0)
            Dim lPoint As New MapWinGIS.Point
            lPoint.x = aX(lPointIndex)
            lPoint.y = aY(lPointIndex)
            lResult = lLine.InsertPoint(lPoint, lPointIndex)
        Next

        lResult = lSf.StartEditingShapes()
        lResult = lSf.EditInsertShape(lLine, lSf.NumShapes)
        lResult = lSf.StopEditingShapes
        pMapWin.View.Redraw()
    End Function

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
                If Not lRetc Then
                    'problem setting value
                    Logger.Dbg("problem setting value to " & aValue)
                End If
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
                                     ByRef aXMax As Double, ByRef aXMin As Double, ByRef aYMax As Double, ByRef aYMin As Double)
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

    ''' <summary>
    ''' Given a point layer and index and a polygon layer, return the index of the polygon this point is in (or -1 if is in none)
    ''' </summary>
    Public Shared Function PointInPolygon(ByVal aPointLayerIndex As Integer, ByVal aPointIndex As Integer, ByVal aPolygonLayerIndex As Integer) As Integer
        Dim lPointSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aPointLayerIndex)
        Return PointInPolygonXY(lPointSf.Shape(aPointIndex).Point(0).x, _
                                lPointSf.Shape(aPointIndex).Point(0).y, _
                                aPolygonLayerIndex)
    End Function

    ''' <summary>
    ''' Given a point xy and a polygon layer, return the index of the polygon this point is in (or -1 if is in none)
    ''' </summary>
    Public Shared Function PointInPolygonXY(ByVal aX As Double, ByVal aY As Double, ByVal aPolygonLayerIndex As Integer) As Integer
        Dim lPolygonSf As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aPolygonLayerIndex)
        lPolygonSf.BeginPointInShapefile()
        PointInPolygonXY = lPolygonSf.PointInShapefile(aX, aY)
        lPolygonSf.EndPointInShapefile()
    End Function

    ''' <summary>
    ''' Given a shape file name, add it to the map; Return true if the layer is already there or successfully added
    ''' </summary>
    Public Shared Function AddLayer(ByVal aFileName As String, _
                                    ByVal aLayerName As String) As Boolean
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
                                    ByRef aAreaGridPoly(,) As Double, _
                                    Optional ByRef aMeanLatGridPoly(,) As Double = Nothing)
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
        'following 3 variables are used in determining mean latitude of each landuse/subbasin combination
        Dim lXLng As Double
        Dim lYLat As Double
        Dim lCountMeanLatLS(aMeanLatGridPoly.GetUpperBound(0), aMeanLatGridPoly.GetUpperBound(1)) As Double

        If pStatusShow Then Logger.Status("Tabulating Areas...")
        For lRow As Integer = lStartingRow To lEndingRow
            If aMeanLatGridPoly IsNot Nothing Then
                Dim lInputProjection As String = GisUtil.ProjectProjection
                Dim lOutputProjection As String = "+proj=longlat +datum=NAD83"
                lInputGrid.CellToProj(lStartingColumn, lRow, lXLng, lYLat)
                GisUtil.ProjectPoint(lXLng, lYLat, lInputProjection, lOutputProjection)  'used for computing mean latitude
            End If
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
                    If aMeanLatGridPoly IsNot Nothing Then
                        If lGridValue > -1 And lGridValue <= aMeanLatGridPoly.GetUpperBound(0) Then
                            'store weighted average mean latitude
                            aMeanLatGridPoly(lGridValue, lInsideId) = (lYLat + (aMeanLatGridPoly(lGridValue, lInsideId) * lCountMeanLatLS(lGridValue, lInsideId))) / (lCountMeanLatLS(lGridValue, lInsideId) + 1)
                            lCountMeanLatLS(lGridValue, lInsideId) += 1
                        End If
                    End If
                End If
                lCellCount += 1
                If pStatusShow Then Logger.Progress(lCellCount, lTotalCellCount)
            Next lCol
        Next lRow

        If pStatusShow Then Logger.Progress("Tabulating Areas...", lTotalCellCount, lTotalCellCount)
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
                    If pStatusShow Then Logger.Progress("Finding max/min grid values in polygon", cellcount, totalcellcount)
                Next lRow
            Next lCol
            If pStatusShow Then Logger.Progress("Finding max/min grid values in polygon", totalcellcount, totalcellcount)
            lPolygonSf.EndPointInShapefile()
        End If
    End Sub

    Public Shared Sub GridMeanInPolygon(ByVal aGridLayerName As String, ByVal aPolygonLayerName As String, _
                                        ByVal aPolygonFeatureIndex As Integer, _
                                        ByRef aMean As Double)
        'Given a grid and a polygon layer, find the mean grid value within the feature.

        Dim lPolygonSf As New MapWinGIS.Shapefile
        If lPolygonSf.Open(aPolygonLayerName) Then
            aMean = ComputeGridMeanInPolygon(aGridLayerName, lPolygonSf, aPolygonFeatureIndex)
            lPolygonSf.Close()
        Else
            aMean = -999
        End If
    End Sub

    Public Shared Sub GridMeanInPolygon(ByVal aGridLayerName As String, ByVal aPolygonLayerIndex As Integer, _
                                        ByVal aPolygonFeatureIndex As Integer, _
                                        ByRef aMean As Double)
        'Given a grid and a polygon layer, find the mean grid value within the feature.

        Dim lPolygonSf As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aPolygonLayerIndex)
        aMean = ComputeGridMeanInPolygon(aGridLayerName, lPolygonSf, aPolygonFeatureIndex)
    End Sub

    Private Shared Function ComputeGridMeanInPolygon(ByVal aGridLayerName As String, ByVal aPolygonShapefile As MapWinGIS.Shapefile, ByVal aPolygonFeatureIndex As Integer) As Double
        'Given a grid and a polygon layer, find the mean grid value within the feature.

        Dim lMean As Double = -999.0
        If FeatureIndexValid(aPolygonFeatureIndex, aPolygonShapefile) Then
            'set input grid
            Dim lInputGrid As New MapWinGIS.Grid
            lInputGrid.Open(aGridLayerName)

            Dim lShape As New MapWinGIS.Shape
            lShape = aPolygonShapefile.Shape(aPolygonFeatureIndex)

            'figure out what part of the grid overlays this polygon
            Dim lStartCol As Integer
            Dim lEndCol As Integer
            Dim lStartRow As Integer
            Dim lEndRow As Integer
            lInputGrid.ProjToCell(lShape.Extents.xMin, lShape.Extents.yMin, lStartCol, lEndRow)
            lInputGrid.ProjToCell(lShape.Extents.xMax, lShape.Extents.yMax, lEndCol, lStartRow)

            Dim lCellcount As Integer = 0

            Dim lSum As Double = 0
            aPolygonShapefile.BeginPointInShapefile()
            For lCol As Integer = lStartCol To lEndCol
                For lRow As Integer = lStartRow To lEndRow
                    Dim lXPos As Double
                    Dim lYPos As Double
                    lInputGrid.CellToProj(lCol, lRow, lXPos, lYPos)
                    Dim lSubId As Integer = aPolygonShapefile.PointInShapefile(lXPos, lYPos)
                    If lSubId = aPolygonFeatureIndex Then 'this is in the polygon we want
                        If Not lInputGrid.Value(lCol, lRow) < 0 Then
                            lSum = lSum + lInputGrid.Value(lCol, lRow)
                            lCellcount = lCellcount + 1
                        End If
                    End If
                Next lRow
            Next lCol
            aPolygonShapefile.EndPointInShapefile()

            lInputGrid.Close()
            lInputGrid = Nothing

            If lCellcount > 0 Then
                lMean = lSum / lCellcount
            End If
        End If
        Return lMean

    End Function

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

    ''' <summary>
    ''' Compute statistics per zone
    ''' </summary>
    ''' <param name="aZoneGridIndex">Index of grid whose values are zone numbers, e.g. subbasins</param>
    ''' <param name="aValueGridIndex">Index of grid to compute statistics from, by zone</param>
    ''' <returns>atcCollection of atcDataAttributes, indexed by zone number. Each atcDataAttributes item contains statistics for one zone.</returns>
    ''' <remarks></remarks>
    Public Shared Function GridZonalStatistics(ByVal aZoneGridIndex As Integer, ByVal aValueGridIndex As Integer) As atcCollection
        Dim lNaN As Double = GetNaN()

        Dim lZoneGrid As MapWinGIS.Grid = GridFromIndex(aZoneGridIndex)
        Dim lValueGrid As MapWinGIS.Grid = GridFromIndex(aValueGridIndex)

        Dim lNodataZone As Integer = lZoneGrid.Header.NodataValue
        Dim lNodataValue As Double = lValueGrid.Header.NodataValue

        Dim lStartRow As Integer = 0
        Dim lStartCol As Integer = 0
        Dim lEndRow As Integer = lZoneGrid.Header.NumberRows - 1
        Dim lEndCol As Integer = lZoneGrid.Header.NumberCols - 1

        Dim lAreaPerCell As Double = lValueGrid.Header.dX * lValueGrid.Header.dY

        If (lValueGrid.Header.NumberRows - 1) <> lEndRow OrElse _
           (lValueGrid.Header.NumberCols - 1) <> lEndCol OrElse _
           Math.Abs(lAreaPerCell - (lZoneGrid.Header.dX * lZoneGrid.Header.dY)) > 0.0001 Then
            Logger.Dbg("GridZonalStatistics: input grids do not match, using GridZonalStatisticsMismatch")
            Return GridZonalStatisticsMismatched(lZoneGrid, lValueGrid)
        End If

        Dim lZoneNumber As Integer
        Dim lValue As Double
        Dim lZonalValues As New atcCollection
        Dim lThisZoneValues As Generic.List(Of Double)

        For lRow As Integer = lStartRow To lEndRow
            For lCol As Integer = lStartCol To lEndCol
                lZoneNumber = lZoneGrid.Value(lCol, lRow)
                If lZoneNumber <> lNodataZone Then
                    lThisZoneValues = lZonalValues.ItemByKey(lZoneNumber)
                    If lThisZoneValues Is Nothing Then
                        lThisZoneValues = New Generic.List(Of Double)
                        lZonalValues.Add(lZoneNumber, lThisZoneValues)
                    End If
                    lValue = lValueGrid.Value(lCol, lRow)
                    If lValue = lNodataValue Then
                        lValue = lNaN
                    End If
                    lThisZoneValues.Add(lValue)
                End If
            Next
            If pStatusShow Then Logger.Progress("Computing zonal statistics", lRow, lEndRow)
        Next

        Dim lZonalStatistics As New atcCollection 'Of ZoneGridIndex, atcDataAttributes

        For lCollectionIndex As Integer = 0 To lZonalValues.Count - 1
            lZoneNumber = lZonalValues.Keys(lCollectionIndex)
            lThisZoneValues = lZonalValues.ItemByIndex(lCollectionIndex)

            'Find most common value, called "Mode", a.k.a. "Majority"
            Dim lValueTotals As New atcCollection
            For Each lValue In lThisZoneValues
                lValueTotals.Increment(lValue)
            Next
            Dim lMode As Double = lNaN
            Dim lModeCount As Integer = 0
            For lSearchIndex As Integer = 0 To lValueTotals.Count - 1
                If lValueTotals.ItemByIndex(lSearchIndex) > lModeCount Then
                    lMode = lValueTotals.Keys(lSearchIndex)
                    lModeCount = lValueTotals.ItemByIndex(lSearchIndex)
                End If
            Next

            Dim lTs As New atcData.atcTimeseries(Nothing)
            lThisZoneValues.Insert(0, lNaN)
            lTs.Values = lThisZoneValues.ToArray
            Dim lNumCellsInZone As Integer = lTs.Attributes.GetValue("Count") + lTs.Attributes.GetValue("Count Missing")
            If Double.IsNaN(lAreaPerCell) OrElse lAreaPerCell <= 0 Then
                lTs.Attributes.SetValue("Number Of Cells", lNumCellsInZone)
                If pStatusShow Then Logger.Dbg("Computed statistics, Zone " & lZoneNumber & " Number Of Cells = " & DoubleToString(lTs.Attributes.GetValue("Number Of Cells")))
            Else
                lTs.Attributes.SetValue("Area", lAreaPerCell * lNumCellsInZone)
                If pStatusShow Then Logger.Dbg("Computed statistics, Zone " & lZoneNumber & " Area = " & DoubleToString(lTs.Attributes.GetValue("Area")))
            End If

            If Not Double.IsNaN(lMode) Then
                lTs.Attributes.SetValue("Mode", lMode)
            End If
            lTs.Attributes.SetValue("Zone", lZoneNumber)
            lZonalStatistics.Add(lZoneNumber, lTs.Attributes)
        Next
        Return lZonalStatistics
    End Function

    ''' <summary>
    ''' Compute statistics per zone
    ''' </summary>
    ''' <param name="aZoneGridIndex">Index of grid whose values are zone numbers, e.g. subbasins</param>
    ''' <param name="aValueGridIndex">Index of grid to compute statistics from, by zone</param>
    ''' <returns>atcCollection of atcDataAttributes, indexed by zone number. Each atcDataAttributes item contains statistics for one zone.</returns>
    ''' <remarks>Grids do not have to be same resolution or area.
    ''' Overlapping area will be used and the finer grid will control the level of detail of the computation.</remarks>
    Public Shared Function GridZonalStatisticsMismatched(ByVal aZoneGridIndex As Integer, ByVal aValueGridIndex As Integer) As atcCollection
        Return GridZonalStatisticsMismatched(GridFromIndex(aZoneGridIndex), GridFromIndex(aValueGridIndex))
    End Function

    Private Shared Function GridZonalStatisticsMismatched(ByVal lZoneGrid As MapWinGIS.Grid, ByVal lValueGrid As MapWinGIS.Grid) As atcCollection
        Dim lNaN As Double = GetNaN()

        Dim lZoneRow As Integer
        Dim lZoneCol As Integer
        Dim lStartZoneRow As Integer = 0
        Dim lStartZoneCol As Integer = 0
        Dim lEndZoneRow As Integer = lZoneGrid.Header.NumberRows - 1
        Dim lEndZoneCol As Integer = lZoneGrid.Header.NumberCols - 1
        Dim lNodataZone As Integer = lZoneGrid.Header.NodataValue

        Dim lValueRow As Integer
        Dim lValueCol As Integer
        Dim lStartValueRow As Integer = 0
        Dim lStartValueCol As Integer = 0
        Dim lEndValueRow As Integer = lValueGrid.Header.NumberRows - 1
        Dim lEndValueCol As Integer = lValueGrid.Header.NumberCols - 1
        Dim lNodataValue As Double = lValueGrid.Header.NodataValue

        GridOverlap(lZoneGrid, lValueGrid, lStartZoneRow, lEndZoneRow, lStartZoneCol, lEndZoneCol, _
                                           lStartValueRow, lEndValueRow, lStartValueCol, lEndValueCol)

        Dim lMapX As Double
        Dim lMapY As Double

        Dim lZoneNumber As Integer
        Dim lValue As Double
        Dim lZonalValues As New atcCollection
        Dim lThisZoneValues As Generic.List(Of Double)

        Dim lAreaPerCell As Double
        Dim lAreaPerValueCell As Double = lValueGrid.Header.dX * lValueGrid.Header.dY
        Dim lAreaPerZoneCell As Double = lZoneGrid.Header.dX * lZoneGrid.Header.dY
        Dim lZoneIsFinest As Boolean = (lAreaPerValueCell >= lAreaPerZoneCell)

        If lZoneIsFinest Then
            lAreaPerCell = lAreaPerZoneCell
            If lAreaPerValueCell = lAreaPerZoneCell Then
                Logger.Dbg("Same cell area in both grids = " & DoubleToString(lAreaPerCell))
            Else
                Logger.Dbg("Using zone cell area = " & DoubleToString(lAreaPerCell) & " < value cell area " & DoubleToString(lAreaPerValueCell))
            End If
            For lZoneRow = lStartZoneRow To lEndZoneRow
                For lZoneCol = lStartZoneCol To lEndZoneCol
                    lZoneNumber = lZoneGrid.Value(lZoneCol, lZoneRow)
                    If lZoneNumber <> lNodataZone Then
                        lThisZoneValues = lZonalValues.ItemByKey(lZoneNumber)
                        If lThisZoneValues Is Nothing Then
                            lThisZoneValues = New Generic.List(Of Double)
                            lZonalValues.Add(lZoneNumber, lThisZoneValues)
                        End If
                        lZoneGrid.CellToProj(lZoneCol, lZoneRow, lMapX, lMapY)
                        lValueGrid.ProjToCell(lMapX, lMapY, lValueCol, lValueRow)
                        If lValueCol < 0 OrElse lValueCol > lEndValueCol OrElse lValueRow < 0 OrElse lValueRow > lEndValueRow Then
                            lValue = lNaN
                        Else
                            lValue = lValueGrid.Value(lValueCol, lValueRow)
                            If lValue = lNodataValue OrElse lValue < -100000 Then 'Treat values less than -100k as Nodata
                                lValue = lNaN
                            End If
                        End If
                        lThisZoneValues.Add(lValue)
                    End If
                Next
                If pStatusShow Then Logger.Progress("Computing zonal statistics", lZoneRow, lEndZoneRow)
            Next
        Else 'Value grid is finer
            lAreaPerCell = lAreaPerValueCell
            Logger.Dbg("Using value cell area = " & DoubleToString(lAreaPerCell) & " < zone cell area " & DoubleToString(lAreaPerZoneCell))
            For lValueRow = lStartValueRow To lEndValueRow
                For lValueCol = lStartValueCol To lEndValueCol
                    lValueGrid.CellToProj(lValueCol, lValueRow, lMapX, lMapY)
                    lZoneGrid.ProjToCell(lMapX, lMapY, lZoneCol, lZoneRow)
                    If lZoneCol < 0 OrElse lZoneCol > lEndZoneCol OrElse lZoneRow < 0 OrElse lZoneRow > lEndZoneRow Then
                        'Skip, not in any zone
                    Else
                        lZoneNumber = lZoneGrid.Value(lZoneCol, lZoneRow)
                        If lZoneNumber <> lNodataZone Then
                            lThisZoneValues = lZonalValues.ItemByKey(lZoneNumber)
                            If lThisZoneValues Is Nothing Then
                                lThisZoneValues = New Generic.List(Of Double)
                                lZonalValues.Add(lZoneNumber, lThisZoneValues)
                            End If
                            lThisZoneValues.Add(lValueGrid.Value(lValueCol, lValueRow))
                        End If
                    End If
                Next
                If pStatusShow Then Logger.Progress("Computing zonal statistics", lValueRow, lEndValueRow)
            Next
        End If

        Dim lZonalStatistics As New atcCollection 'Of ZoneGridIndex, atcDataAttributes

        For lCollectionIndex As Integer = 0 To lZonalValues.Count - 1
            lZoneNumber = lZonalValues.Keys(lCollectionIndex)
            lThisZoneValues = lZonalValues.ItemByIndex(lCollectionIndex)

            'Find most common value, called "Mode", a.k.a. "Majority"
            Dim lValueTotals As New atcCollection
            For Each lValue In lThisZoneValues
                lValueTotals.Increment(lValue)
            Next
            Dim lMode As Double = lNaN
            Dim lModeCount As Integer = 0
            For lSearchIndex As Integer = 0 To lValueTotals.Count - 1
                If lValueTotals.ItemByIndex(lSearchIndex) > lModeCount Then
                    lMode = lValueTotals.Keys(lSearchIndex)
                    lModeCount = lValueTotals.ItemByIndex(lSearchIndex)
                End If
            Next

            Dim lTs As New atcData.atcTimeseries(Nothing)
            lThisZoneValues.Insert(0, lNaN)
            lTs.Values = lThisZoneValues.ToArray
            Dim lNumCellsInZone As Integer = lTs.Attributes.GetValue("Count") + lTs.Attributes.GetValue("Count Missing")
            If Double.IsNaN(lAreaPerCell) OrElse lAreaPerCell <= 0 Then
                lTs.Attributes.SetValue("Number Of Cells", lNumCellsInZone)
                If pStatusShow Then Logger.Dbg("Computed statistics, Zone " & lZoneNumber & " Number Of Cells = " & DoubleToString(lTs.Attributes.GetValue("Number Of Cells")))
            Else
                lTs.Attributes.SetValue("Area", lAreaPerCell * lNumCellsInZone)
                If pStatusShow Then Logger.Dbg("Computed statistics, Zone " & lZoneNumber & " Area = " & DoubleToString(lTs.Attributes.GetValue("Area")))
            End If

            If Not Double.IsNaN(lMode) Then
                lTs.Attributes.SetValue("Mode", lMode)
            End If
            lTs.Attributes.SetValue("Zone", lZoneNumber)
            lZonalStatistics.Add(lZoneNumber, lTs.Attributes)
        Next
        Return lZonalStatistics
    End Function

    Public Shared Function GridZoneCountValue(ByVal aZoneGridIndex As Integer, ByVal aValueGridIndex As Integer, ByVal aValue As Integer) As atcCollection
        Dim lZoneGrid As MapWinGIS.Grid = GridFromIndex(aZoneGridIndex)
        Dim lValueGrid As MapWinGIS.Grid = GridFromIndex(aValueGridIndex)

        Dim lStartRow As Integer = 0
        Dim lStartCol As Integer = 0
        Dim lEndRow As Integer = lZoneGrid.Header.NumberRows - 1
        Dim lEndCol As Integer = lZoneGrid.Header.NumberCols - 1

        Dim lNodataZone As Integer = lZoneGrid.Header.NodataValue
        Dim lNodataValue As Double = lValueGrid.Header.NodataValue

        Dim lZoneNumber As Integer
        Dim lZonalCounts As New atcCollection

        For lRow As Integer = lStartRow To lEndRow
            For lCol As Integer = lStartCol To lEndCol
                If lValueGrid.Value(lCol, lRow) = aValue Then
                    'increment count for this zone
                    lZoneNumber = lZoneGrid.Value(lCol, lRow)
                    If lZoneNumber <> lNodataZone Then
                        If lZonalCounts.ItemByKey(lZoneNumber) Is Nothing Then
                            lZonalCounts.Add(lZoneNumber, 1)
                        Else
                            lZonalCounts.ItemByKey(lZoneNumber) += 1
                        End If
                    End If
                End If
            Next
        Next

        Return lZonalCounts
    End Function

    Private Shared Sub GridOverlap(ByVal aGrid1 As MapWinGIS.Grid, ByVal aGrid2 As MapWinGIS.Grid, _
        ByRef aStartRow1 As Integer, ByVal aEndRow1 As Integer, ByRef aStartCol1 As Integer, ByVal aEndCol1 As Integer, _
        ByRef aStartRow2 As Integer, ByVal aEndRow2 As Integer, ByRef aStartCol2 As Integer, ByVal aEndCol2 As Integer)

        Dim ldX1 As Double = aGrid1.Header.dX
        Dim ldY1 As Double = aGrid1.Header.dY
        Dim ldX2 As Double = aGrid2.Header.dX
        Dim ldY2 As Double = aGrid2.Header.dY

        'Start with assumption that all rows and columns of both overlap
        aStartRow1 = 0 : aStartRow2 = 0 : aStartCol1 = 0 : aStartCol2 = 0

        aEndRow1 = aGrid1.Header.NumberRows - 1
        aEndCol1 = aGrid1.Header.NumberCols - 1

        aEndRow2 = aGrid2.Header.NumberRows - 1
        aEndCol2 = aGrid2.Header.NumberCols - 1

        Dim lGrid1LeftX, lGrid1RightX, lGrid1TopY, lGrid1BottomY As Double
        Dim lGrid2LeftX, lGrid2RightX, lGrid2TopY, lGrid2BottomY As Double

        'Find projected location of edges of each grid
        aGrid1.CellToProj(aStartCol1, aStartRow1, lGrid1LeftX, lGrid1TopY)
        aGrid2.CellToProj(aStartCol2, aStartRow2, lGrid2LeftX, lGrid2TopY)

        aGrid1.CellToProj(aEndCol1, aEndRow1, lGrid1RightX, lGrid1BottomY)
        aGrid2.CellToProj(aEndCol2, aEndRow2, lGrid2RightX, lGrid2BottomY)

        If (lGrid1LeftX < lGrid1RightX) AndAlso (lGrid1TopY > lGrid1BottomY) Then
            If lGrid1LeftX < lGrid2LeftX Then
                aStartCol1 += Math.Floor((lGrid2LeftX - lGrid1LeftX) / ldX1)
            Else
                aStartCol2 += Math.Floor((lGrid1LeftX - lGrid2LeftX) / ldX2)
            End If

            If lGrid1RightX > lGrid2RightX Then
                aEndCol1 -= Math.Floor((lGrid2RightX - lGrid1RightX) / ldX1)
            Else
                aEndCol2 -= Math.Floor((lGrid1RightX - lGrid2RightX) / ldX2)
            End If

            If lGrid1TopY > lGrid2TopY Then
                aStartRow1 += Math.Floor((lGrid1TopY - lGrid2TopY) / ldY1)
            Else
                aStartRow2 += Math.Floor((lGrid2TopY - lGrid1TopY) / ldY2)
            End If

            If lGrid1BottomY < lGrid2BottomY Then
                aEndRow1 -= Math.Floor((lGrid2BottomY - lGrid1BottomY) / ldY1)
            Else
                aEndRow2 -= Math.Floor((lGrid1BottomY - lGrid2BottomY) / ldY2)
            End If
        Else
            Throw New ApplicationException("Grids are not in expected orientation, additional code needed in GridOverlap")
        End If
    End Sub

    Public Shared Sub GridAssignValues(ByVal aStreamGridFileName As String, ByVal aSubbasinGridFileName As String, ByVal aNewGridFileName As String)
        'to the first grid, assign the values at the coresponding cells of the second grid, and save the result as the third.
        'used to assign a subbasin number to each stream cell

        Dim lStreamGridLayerIndex As Integer = GisUtil.LayerIndex(aStreamGridFileName)
        Dim lSubbasinGridLayerIndex As Integer = GisUtil.LayerIndex(aSubbasinGridFileName)

        Dim lStreamGrid As MapWinGIS.Grid = GridFromIndex(lStreamGridLayerIndex)
        Dim lSubbasinGrid As MapWinGIS.Grid = GridFromIndex(lSubbasinGridLayerIndex)

        If FileExists(aNewGridFileName) Then
            IO.File.Delete(aNewGridFileName)
        End If
        IO.File.Copy(aStreamGridFileName, aNewGridFileName)
        Dim lOutputGrid As New MapWinGIS.Grid
        lOutputGrid.Open(aNewGridFileName)

        Dim lStartRow As Integer = 0
        Dim lStartCol As Integer = 0
        Dim lEndRow As Integer = lStreamGrid.Header.NumberRows - 1
        Dim lEndCol As Integer = lStreamGrid.Header.NumberCols - 1

        If (lSubbasinGrid.Header.NumberRows - 1) <> lEndRow Then
            Throw New ApplicationException("GridAssignValues: Number of rows in input grids do not match: " & lStreamGrid.Header.NumberRows & " <> " & lSubbasinGrid.Header.NumberRows)
        End If
        If (lSubbasinGrid.Header.NumberCols - 1) <> lEndCol Then
            Throw New ApplicationException("GridAssignValues: Number of columns in input grids do not match: " & lStreamGrid.Header.NumberCols & " <> " & lSubbasinGrid.Header.NumberCols)
        End If

        Dim lNoData As Object = lStreamGrid.Header.NodataValue
        Dim lTempVal As Object = Nothing

        For lRow As Integer = lStartRow To lEndRow
            For lCol As Integer = lStartCol To lEndCol
                lTempVal = lStreamGrid.Value(lCol, lRow)
                If lTempVal <> lNoData And lTempVal > 0 Then
                    lOutputGrid.Value(lCol, lRow) = lSubbasinGrid.Value(lCol, lRow)
                Else
                    lOutputGrid.Value(lCol, lRow) = lNoData
                End If
            Next
        Next

        lOutputGrid.Save()
    End Sub

    Public Shared Sub GridBuildOutlets(ByVal aStreamLinkGridFileName As String, ByVal aFlowAccGridFileName As String, ByVal aNewGridFileName As String)
        'used in creating an outlets grid.  for each stream link, include only the cell with the maximum flow accumulation

        Dim lStreamGridLayerIndex As Integer = GisUtil.LayerIndex(aStreamLinkGridFileName)
        Dim lFlowAccGridLayerIndex As Integer = GisUtil.LayerIndex(aFlowAccGridFileName)

        Dim lStreamGrid As MapWinGIS.Grid = GridFromIndex(lStreamGridLayerIndex)
        Dim lFlowAccGrid As MapWinGIS.Grid = GridFromIndex(lFlowAccGridLayerIndex)

        If FileExists(aNewGridFileName) Then
            IO.File.Delete(aNewGridFileName)
        End If
        IO.File.Copy(aStreamLinkGridFileName, aNewGridFileName)
        Dim lOutputGrid As New MapWinGIS.Grid
        lOutputGrid.Open(aNewGridFileName)

        Dim lStartRow As Integer = 0
        Dim lStartCol As Integer = 0
        Dim lEndRow As Integer = lStreamGrid.Header.NumberRows - 1
        Dim lEndCol As Integer = lStreamGrid.Header.NumberCols - 1

        If (lFlowAccGrid.Header.NumberRows - 1) <> lEndRow Then
            Throw New ApplicationException("GridBuildOutlets: Number of rows in input grids do not match: " & lStreamGrid.Header.NumberRows & " <> " & lFlowAccGrid.Header.NumberRows)
        End If
        If (lFlowAccGrid.Header.NumberCols - 1) <> lEndCol Then
            Throw New ApplicationException("GridBuildOutlets: Number of columns in input grids do not match: " & lStreamGrid.Header.NumberCols & " <> " & lFlowAccGrid.Header.NumberCols)
        End If

        Dim lNoData As Object = lStreamGrid.Header.NodataValue
        Dim lStreamVal As Object = Nothing
        Dim lFlowAccVal As Object = Nothing

        Dim lMaxAccums As New atcCollection

        'in first pass figure out the highest flowacc in each stream segment 
        For lRow As Integer = lStartRow To lEndRow
            For lCol As Integer = lStartCol To lEndCol
                lStreamVal = lStreamGrid.Value(lCol, lRow)
                If lStreamVal > 0 Then
                    'this is a stream segment 
                    lFlowAccVal = lFlowAccGrid.Value(lCol, lRow)
                    If Not lMaxAccums.Keys.Contains(lStreamVal) Then
                        lMaxAccums.Add(lStreamVal, lFlowAccVal)
                    Else
                        If lMaxAccums.ItemByKey(lStreamVal) < lFlowAccVal Then
                            lMaxAccums.ItemByKey(lStreamVal) = lFlowAccVal
                        End If
                    End If
                End If
            Next
        Next

        'in second pass set to null every cell except the one with the highest flowacc in each stream segment 
        For lRow As Integer = lStartRow To lEndRow
            For lCol As Integer = lStartCol To lEndCol
                lStreamVal = lStreamGrid.Value(lCol, lRow)
                If lStreamVal > 0 Then
                    'this is a stream segment 
                    lFlowAccVal = lFlowAccGrid.Value(lCol, lRow)
                    If lMaxAccums.ItemByKey(lStreamVal) <> lFlowAccVal Then
                        lOutputGrid.Value(lCol, lRow) = lNoData
                    End If
                End If
            Next
        Next

        lOutputGrid.Save()
    End Sub

    Public Shared Sub GridAssignValuesToNull(ByVal aInputGridFileName As String, ByVal aStreamGridFileName As String, ByVal aNewGridFileName As String)
        'to the first grid, assign null at the corresponding cells of the second grid, and save the result as the third.
        'used to make null all the stream cells in a flow dir grid

        Dim lInputGridLayerIndex As Integer = GisUtil.LayerIndex(aInputGridFileName)
        Dim lStreamGridLayerIndex As Integer = GisUtil.LayerIndex(aStreamGridFileName)

        Dim lInputGrid As MapWinGIS.Grid = GridFromIndex(lInputGridLayerIndex)
        Dim lStreamGrid As MapWinGIS.Grid = GridFromIndex(lStreamGridLayerIndex)

        If FileExists(aNewGridFileName) Then
            IO.File.Delete(aNewGridFileName)
        End If
        IO.File.Copy(aInputGridFileName, aNewGridFileName)
        Dim lOutputGrid As New MapWinGIS.Grid
        lOutputGrid.Open(aNewGridFileName)

        Dim lStartRow As Integer = 0
        Dim lStartCol As Integer = 0
        Dim lEndRow As Integer = lInputGrid.Header.NumberRows - 1
        Dim lEndCol As Integer = lInputGrid.Header.NumberCols - 1

        If (lStreamGrid.Header.NumberRows - 1) <> lEndRow Then
            Throw New ApplicationException("GridAssignValuesNull: Number of rows in input grids do not match: " & lInputGrid.Header.NumberRows & " <> " & lStreamGrid.Header.NumberRows)
        End If
        If (lStreamGrid.Header.NumberCols - 1) <> lEndCol Then
            Throw New ApplicationException("GridAssignValuesNull: Number of columns in input grids do not match: " & lInputGrid.Header.NumberCols & " <> " & lStreamGrid.Header.NumberCols)
        End If

        Dim lNoData As Object = lInputGrid.Header.NodataValue
        Dim lTempVal As Object = Nothing

        For lRow As Integer = lStartRow To lEndRow
            For lCol As Integer = lStartCol To lEndCol
                lTempVal = lStreamGrid.Value(lCol, lRow)
                If lTempVal <> lNoData And lTempVal > 0 Then
                    lOutputGrid.Value(lCol, lRow) = lNoData
                End If
            Next
        Next

        lOutputGrid.Save()
    End Sub

    Public Shared Sub GridAssignConstant(ByVal aNewGridFileName As String, ByVal aBaseGridFileName As String, ByVal aValue As Double)
        If FileExists(aNewGridFileName) Then
            IO.File.Delete(aNewGridFileName)
        End If
        IO.File.Copy(aBaseGridFileName, aNewGridFileName)

        Dim lOutputGrid As New MapWinGIS.Grid
        lOutputGrid.Open(aNewGridFileName)

        Dim lStartRow As Integer = 0
        Dim lStartCol As Integer = 0
        Dim lEndRow As Integer = lOutputGrid.Header.NumberRows - 1
        Dim lEndCol As Integer = lOutputGrid.Header.NumberCols - 1

        For lRow As Integer = lStartRow To lEndRow
            For lCol As Integer = lStartCol To lEndCol
                lOutputGrid.Value(lCol, lRow) = aValue
            Next
        Next

        lOutputGrid.Save()
    End Sub

    Public Shared Sub GridAssignConstantAboveThreshold(ByVal aNewGridFileName As String, ByVal aThresholdGridFileName As String, ByVal aThreshold As Double, ByVal aValue As Double)
        Dim lOutputGrid As New MapWinGIS.Grid
        lOutputGrid.Open(aNewGridFileName)

        Dim lThreshGrid As New MapWinGIS.Grid
        lThreshGrid.Open(aThresholdGridFileName)

        Dim lStartRow As Integer = 0
        Dim lStartCol As Integer = 0
        Dim lEndRow As Integer = lOutputGrid.Header.NumberRows - 1
        Dim lEndCol As Integer = lOutputGrid.Header.NumberCols - 1

        For lRow As Integer = lStartRow To lEndRow
            For lCol As Integer = lStartCol To lEndCol
                If lThreshGrid.Value(lCol, lRow) > aThreshold Then
                    lOutputGrid.Value(lCol, lRow) = aValue
                End If
            Next
        Next

        lOutputGrid.Save()
    End Sub

    Public Shared Sub GridInverse(ByVal aInputGridFileName As String, ByVal aNewGridFileName As String)
        If FileExists(aNewGridFileName) Then
            IO.File.Delete(aNewGridFileName)
        End If
        IO.File.Copy(aInputGridFileName, aNewGridFileName)

        Dim lOutputGrid As New MapWinGIS.Grid
        lOutputGrid.Open(aNewGridFileName)

        Dim lStartRow As Integer = 0
        Dim lStartCol As Integer = 0
        Dim lEndRow As Integer = lOutputGrid.Header.NumberRows - 1
        Dim lEndCol As Integer = lOutputGrid.Header.NumberCols - 1

        For lRow As Integer = lStartRow To lEndRow
            For lCol As Integer = lStartCol To lEndCol
                lOutputGrid.Value(lCol, lRow) = 1 / lOutputGrid.Value(lCol, lRow)
            Next
        Next

        lOutputGrid.Save()
    End Sub

    Public Shared Sub GridMultiplyConstant(ByVal aInputGridFileName As String, ByVal aValue As Double, ByVal aNewGridFileName As String)
        If FileExists(aNewGridFileName) Then
            IO.File.Delete(aNewGridFileName)
        End If
        IO.File.Copy(aInputGridFileName, aNewGridFileName)

        Dim lOutputGrid As New MapWinGIS.Grid
        lOutputGrid.Open(aNewGridFileName)

        Dim lStartRow As Integer = 0
        Dim lStartCol As Integer = 0
        Dim lEndRow As Integer = lOutputGrid.Header.NumberRows - 1
        Dim lEndCol As Integer = lOutputGrid.Header.NumberCols - 1

        For lRow As Integer = lStartRow To lEndRow
            For lCol As Integer = lStartCol To lEndCol
                lOutputGrid.Value(lCol, lRow) = lOutputGrid.Value(lCol, lRow) * aValue
            Next
        Next

        lOutputGrid.Save()
    End Sub

    Public Shared Sub GridMultiply(ByVal aInputGridFileName As String, ByVal aInput2GridFileName As String, ByVal aNewGridFileName As String)
        If FileExists(aNewGridFileName) Then
            IO.File.Delete(aNewGridFileName)
        End If
        IO.File.Copy(aInputGridFileName, aNewGridFileName)

        Dim lOutputGrid As New MapWinGIS.Grid
        lOutputGrid.Open(aNewGridFileName)

        Dim lInput2Grid As New MapWinGIS.Grid
        lInput2Grid.Open(aInput2GridFileName)

        Dim lStartRow As Integer = 0
        Dim lStartCol As Integer = 0
        Dim lEndRow As Integer = lOutputGrid.Header.NumberRows - 1
        Dim lEndCol As Integer = lOutputGrid.Header.NumberCols - 1

        For lRow As Integer = lStartRow To lEndRow
            For lCol As Integer = lStartCol To lEndCol
                If lOutputGrid.Value(lCol, lRow) <> lOutputGrid.Header.NodataValue Then
                    lOutputGrid.Value(lCol, lRow) = lOutputGrid.Value(lCol, lRow) * lInput2Grid.Value(lCol, lRow)
                End If
            Next
        Next

        lOutputGrid.Save()
    End Sub

    Public Shared Sub GridToInteger(ByVal aInputGridFileName As String, ByVal aNewGridFileName As String)
        If FileExists(aNewGridFileName) Then
            IO.File.Delete(aNewGridFileName)
        End If

        Dim lInputGrid As New MapWinGIS.Grid
        lInputGrid.Open(aInputGridFileName)

        Dim lOutputGrid As New MapWinGIS.Grid
        lOutputGrid.CreateNew(aNewGridFileName, lInputGrid.Header, MapWinGIS.GridDataType.ShortDataType, -999)
        lOutputGrid.Header.NodataValue = -999

        Dim lStartRow As Integer = 0
        Dim lStartCol As Integer = 0
        Dim lEndRow As Integer = lOutputGrid.Header.NumberRows - 1
        Dim lEndCol As Integer = lOutputGrid.Header.NumberCols - 1

        For lRow As Integer = lStartRow To lEndRow
            For lCol As Integer = lStartCol To lEndCol
                If lInputGrid.Value(lCol, lRow) > -1 Then
                    lOutputGrid.Value(lCol, lRow) = lInputGrid.Value(lCol, lRow)
                End If
            Next
        Next

        lOutputGrid.Save()
    End Sub

    Public Shared Sub GridSetNoData(ByVal aInputGridFileName As String, ByVal aThreshold As Double)
        'set no data values in the input grid, save it 

        Dim lInputGridLayerIndex As Integer = GisUtil.LayerIndex(aInputGridFileName)
        Dim lInputGrid As MapWinGIS.Grid = GridFromIndex(lInputGridLayerIndex)

        Dim lNoData As Double = lInputGrid.Header.NodataValue
        Dim lMinValue As Double = lInputGrid.Value(0, 0)

        Dim lStartRow As Integer = 0
        Dim lStartCol As Integer = 0
        Dim lEndRow As Integer = lInputGrid.Header.NumberRows - 1
        Dim lEndCol As Integer = lInputGrid.Header.NumberCols - 1

        Dim lTempVal As Double = 0.0
        For lRow As Integer = lStartRow To lEndRow
            For lCol As Integer = lStartCol To lEndCol
                lTempVal = lInputGrid.Value(lCol, lRow)
                If lTempVal < lMinValue Then
                    lMinValue = lTempVal
                End If
            Next
        Next

        If lMinValue < aThreshold And lMinValue <> lNoData Then
            'arbitrary rule to determine if the nodata value is incorrectly set on this grid
            lInputGrid.Header.NodataValue = lMinValue
            lInputGrid.Save()
        End If

    End Sub

    Public Shared Sub GridFromShapefile(ByVal aShapefileLayerIndex As Integer, ByVal aShapefileFieldIndex As Integer, ByVal aBaseGridFileName As String, ByVal aOutputGridFileName As String)
        'create a grid from a shapefile,
        'given a shapefile, a field index, and a base grid
        Dim lShapefileName As String = LayerFileName(aShapefileLayerIndex)
        Dim lGrid As New MapWinGIS.Grid
        lGrid.Open(aBaseGridFileName)
        Dim lFieldName As String = FieldName(aShapefileFieldIndex, aShapefileLayerIndex)

        MapWinGeoProc.Utils.ShapefileToGrid(lShapefileName, aOutputGridFileName, MapWinGIS.GridFileType.UseExtension, lGrid.DataType, lFieldName, lGrid.Header.dX, Nothing)
    End Sub

    Public Shared Sub DownstreamFlowLength(ByVal aFlowDirGridFileName As String, ByVal aFlowAccGridFileName As String, ByVal aLenGridFileName As String, _
                                           Optional ByVal aStreamGridFileName As String = "", Optional ByVal aWeightingFactorGridName As String = "")
        'new sub to compute downstream flow length, not available directly in mapwindow
        '  if stream grid is nothing, compute distances to watershed outlet.
        '  if stream grid provided, compute distances to nearest stream.

        'prepare the output grid
        If FileExists(aLenGridFileName) Then
            IO.File.Delete(aLenGridFileName)
        End If
        IO.File.Copy(aFlowAccGridFileName, aLenGridFileName)

        Dim lOutputGrid As New MapWinGIS.Grid
        lOutputGrid.Open(aLenGridFileName)

        'initialize the new grid
        Dim lNoData As Double = lOutputGrid.Header.NodataValue
        For lRow As Integer = 0 To lOutputGrid.Header.NumberRows - 1
            For lCol As Integer = 0 To lOutputGrid.Header.NumberCols - 1
                lOutputGrid.Value(lCol, lRow) = lNoData
            Next
        Next

        'prepare the flow dir grid
        Dim lFlowDirGrid As New MapWinGIS.Grid
        lFlowDirGrid.Open(aFlowDirGridFileName)

        Dim lStartRow As Integer = 0
        Dim lStartCol As Integer = 0
        Dim lEndRow As Integer = 0
        Dim lEndCol As Integer = 0
        Dim lTempVal As Double = 0.0

        If aStreamGridFileName.Length = 0 Then
            'use the flow acc grid to find the outlet of the watershed 
            Dim lFlowAccGrid As New MapWinGIS.Grid
            lFlowAccGrid.Open(aFlowAccGridFileName)

            lEndRow = lFlowAccGrid.Header.NumberRows - 1
            lEndCol = lFlowAccGrid.Header.NumberCols - 1
            Dim lMaxRow As Integer = -1
            Dim lMaxCol As Integer = -1
            Dim lMaxVal As Double = 0.0

            For lRow As Integer = lStartRow To lEndRow
                For lCol As Integer = lStartCol To lEndCol
                    lTempVal = lFlowAccGrid.Value(lCol, lRow)
                    If lTempVal > lMaxVal Then
                        lMaxVal = lTempVal
                        lMaxRow = lRow
                        lMaxCol = lCol
                    End If
                Next
            Next
            lFlowAccGrid = Nothing
            lOutputGrid.Value(lMaxCol, lMaxRow) = 0.0

            'now call recursive routine to set surrounding cells 
            Downstream.CheckNeighboringCells(lFlowDirGrid, lMaxCol, lMaxRow, lOutputGrid)
        Else
            'use the streams grid to compute the distance to a stream cell
            'or outlets grid to compute the distance to an outlet
            Dim lStreamGrid As New MapWinGIS.Grid
            lStreamGrid.Open(aStreamGridFileName)

            Dim lWeightingFactorGrid As New MapWinGIS.Grid
            If aWeightingFactorGridName.Length > 0 Then
                lWeightingFactorGrid.Open(aWeightingFactorGridName)
            Else
                lWeightingFactorGrid = Nothing
            End If

            lEndRow = lStreamGrid.Header.NumberRows - 1
            lEndCol = lStreamGrid.Header.NumberCols - 1

            'store orig flow dir in an array for later use
            Dim lFlowDirArray(lEndCol, lEndRow) As Integer
            For lRow As Integer = lStartRow To lEndRow
                For lCol As Integer = lStartCol To lEndCol
                    lFlowDirArray(lCol, lRow) = lFlowDirGrid.Value(lCol, lRow)
                    lTempVal = lStreamGrid.Value(lCol, lRow)
                    If lTempVal > 0 Then
                        'set the flow dir to zero
                        lFlowDirGrid.Value(lCol, lRow) = 0.0
                    End If
                Next
            Next

            Dim lFlowAccGrid As New MapWinGIS.Grid
            lFlowAccGrid.Open(aFlowAccGridFileName)
            'store orig flow acc in an array for later use
            Dim lFlowAccArray(lEndCol, lEndRow) As Integer
            For lRow As Integer = lStartRow To lEndRow
                For lCol As Integer = lStartCol To lEndCol
                    If lFlowAccGrid.Value(lCol, lRow) > -1 Then
                        lFlowAccArray(lCol, lRow) = lFlowAccGrid.Value(lCol, lRow)
                    End If
                Next
            Next

            'this is the assumed flow dir coding from taudem:
            '4 3 2
            '5   1
            '6 7 8
            Dim lnw As Integer = 8
            Dim ln As Integer = 7
            Dim lne As Integer = 6
            Dim lw As Integer = 1
            Dim le As Integer = 5
            Dim lsw As Integer = 2
            Dim ls As Integer = 3
            Dim lse As Integer = 4

            If lFlowDirGrid.Maximum > 8 Then
                'assume arcview coding:
                '32 64 128
                '16      1
                ' 8  4   2
                lnw = 2
                ln = 4
                lne = 8
                lw = 1
                le = 16
                lsw = 128
                ls = 64
                lse = 32
            End If

            Dim lDistanceFactor As Double = Math.Sqrt((lOutputGrid.Header.dX ^ 2) + (lOutputGrid.Header.dY ^ 2))
            Dim lDistanceFactorX As Double = lOutputGrid.Header.dX
            Dim lDistanceFactorY As Double = lOutputGrid.Header.dY
            Dim lDistanceFactorD As Double = Math.Sqrt((lOutputGrid.Header.dX ^ 2) + (lOutputGrid.Header.dY ^ 2))
            For lRow As Integer = lStartRow To lEndRow
                For lCol As Integer = lStartCol To lEndCol
                    lTempVal = lStreamGrid.Value(lCol, lRow)
                    If lTempVal > 0 Then
                        'this is actually an outlet cell or a stream cell
                        If lWeightingFactorGrid Is Nothing Then
                            lOutputGrid.Value(lCol, lRow) = 0.0
                        Else
                            'need to know flow dir of cell upstream of this one
                            Dim lUpCol As Integer = 0
                            Dim lUpRow As Integer = 0
                            Dim lBaseAcc As Integer = lFlowAccArray(lCol, lRow)
                            Dim lGreatestAcc As Integer = 0
                            Dim lUpFlowDir As Integer = 0
                            'check all 8 surrounding cells
                            If lFlowAccArray(lCol - 1, lRow) < lBaseAcc And lFlowAccArray(lCol - 1, lRow) > lGreatestAcc Then
                                lUpFlowDir = lFlowDirArray(lCol - 1, lRow)
                                lGreatestAcc = lFlowAccArray(lCol - 1, lRow)
                            End If
                            If lFlowAccArray(lCol - 1, lRow - 1) < lBaseAcc And lFlowAccArray(lCol - 1, lRow - 1) > lGreatestAcc Then
                                lUpFlowDir = lFlowDirArray(lCol - 1, lRow - 1)
                                lGreatestAcc = lFlowAccArray(lCol - 1, lRow - 1)
                            End If
                            If lFlowAccArray(lCol, lRow - 1) < lBaseAcc And lFlowAccArray(lCol, lRow - 1) > lGreatestAcc Then
                                lUpFlowDir = lFlowDirArray(lCol, lRow - 1)
                                lGreatestAcc = lFlowAccArray(lCol, lRow - 1)
                            End If
                            If lFlowAccArray(lCol + 1, lRow) < lBaseAcc And lFlowAccArray(lCol + 1, lRow) > lGreatestAcc Then
                                lUpFlowDir = lFlowDirArray(lCol + 1, lRow)
                                lGreatestAcc = lFlowAccArray(lCol + 1, lRow)
                            End If
                            If lFlowAccArray(lCol + 1, lRow + 1) < lBaseAcc And lFlowAccArray(lCol + 1, lRow + 1) > lGreatestAcc Then
                                lUpFlowDir = lFlowDirArray(lCol + 1, lRow + 1)
                                lGreatestAcc = lFlowAccArray(lCol + 1, lRow + 1)
                            End If
                            If lFlowAccArray(lCol, lRow + 1) < lBaseAcc And lFlowAccArray(lCol, lRow + 1) > lGreatestAcc Then
                                lUpFlowDir = lFlowDirArray(lCol, lRow + 1)
                                lGreatestAcc = lFlowAccArray(lCol, lRow + 1)
                            End If
                            If lFlowAccArray(lCol - 1, lRow + 1) < lBaseAcc And lFlowAccArray(lCol - 1, lRow + 1) > lGreatestAcc Then
                                lUpFlowDir = lFlowDirArray(lCol - 1, lRow + 1)
                                lGreatestAcc = lFlowAccArray(lCol - 1, lRow + 1)
                            End If
                            If lFlowAccArray(lCol + 1, lRow - 1) < lBaseAcc And lFlowAccArray(lCol + 1, lRow - 1) > lGreatestAcc Then
                                lUpFlowDir = lFlowDirArray(lCol + 1, lRow - 1)
                                lGreatestAcc = lFlowAccArray(lCol + 1, lRow - 1)
                            End If
                            'now compute the distance factor based on that
                            If lUpFlowDir = lw Or lUpFlowDir = le Then
                                lDistanceFactor = lDistanceFactorX
                            ElseIf lUpFlowDir = ln Or lUpFlowDir = ls Then
                                lDistanceFactor = lDistanceFactorY
                            Else
                                'flowing in the diagonal 
                                lDistanceFactor = lDistanceFactorD
                            End If
                            lOutputGrid.Value(lCol, lRow) = -1.0 * lDistanceFactor * lWeightingFactorGrid.Value(lCol, lRow)
                        End If
                        'now call recursive routine to set surrounding cells 
                        Downstream.CheckNeighboringCells(lFlowDirGrid, lCol, lRow, lOutputGrid, lWeightingFactorGrid)
                    End If
                Next
                If pStatusShow Then Logger.Progress("Computing Downstream Flow Length", lRow, lEndRow - lStartRow + 1)
            Next
            lStreamGrid.Close()
            lStreamGrid = Nothing

            If lWeightingFactorGrid IsNot Nothing Then
                lWeightingFactorGrid.Close()
                lWeightingFactorGrid = Nothing
            End If
        End If

        lOutputGrid.Save()
        lOutputGrid = Nothing

        lFlowDirGrid.Close()
        lFlowDirGrid = Nothing
    End Sub

    Public Shared Sub FlowLengthToOutlet(ByVal aLenGridFileName As String, ByVal aOutletGridFileName As String, ByVal aZoneGridFileName As String, ByVal aOutputGridFileName As String)
        'compute downstream flow length to outlet of each subbasin

        'prepare the output grid
        If FileExists(aOutputGridFileName) Then
            IO.File.Delete(aOutputGridFileName)
        End If
        IO.File.Copy(aLenGridFileName, aOutputGridFileName)
        Dim lOutputGrid As New MapWinGIS.Grid
        lOutputGrid.Open(aOutputGridFileName)

        'prepare the flow length grid
        Dim lFlowLenGrid As New MapWinGIS.Grid
        lFlowLenGrid.Open(aLenGridFileName)

        'for each outlet, find the corresponding flow length
        Dim lOutletGrid As New MapWinGIS.Grid
        lOutletGrid.Open(aOutletGridFileName)
        Dim lEndRow As Integer = lOutletGrid.Header.NumberRows - 1
        Dim lEndCol As Integer = lOutletGrid.Header.NumberCols - 1
        Dim lBasinVal As Integer = -1
        Dim lFlowLen As Double = -1.0
        Dim lSubbasinFlowLengths As New atcCollection
        For lRow As Integer = 0 To lEndRow
            For lCol As Integer = 0 To lEndCol
                lBasinVal = lOutletGrid.Value(lCol, lRow)
                If lBasinVal > 0 Then
                    'this is a subbasin outlet, get flow length at this location
                    lFlowLen = lFlowLenGrid.Value(lCol, lRow)
                    lSubbasinFlowLengths.Add(lBasinVal, lFlowLen)
                End If
            Next
        Next

        'prepare the zone grid
        Dim lZoneGrid As New MapWinGIS.Grid
        lZoneGrid.Open(aZoneGridFileName)

        'loop through cells of output grid
        lEndRow = lOutputGrid.Header.NumberRows - 1
        lEndCol = lOutputGrid.Header.NumberCols - 1
        For lRow As Integer = 0 To lEndRow
            For lCol As Integer = 0 To lEndCol
                lBasinVal = lZoneGrid.Value(lCol, lRow)
                If lBasinVal > 0 Then
                    'this is a subbasin, adjust flow length at this location
                    lOutputGrid.Value(lCol, lRow) = lOutputGrid.Value(lCol, lRow) - lSubbasinFlowLengths.ItemByKey(lBasinVal) - lOutputGrid.Header.dX
                End If
            Next
        Next

        lOutputGrid.Save()
        lOutputGrid = Nothing
    End Sub

    Public Shared Sub GridDownstreamSubbasins(ByVal aOutletGridFileName As String, ByVal aFlowDirGridFileName As String, ByVal aSubbasinGridFileName As String, ByVal aDownstreamGridFileName As String)
        'given an outlet grid, a flow dir grid, and a subbasins grid, build a downstream subbasin grid
        Dim lOutletGridLayerIndex As Integer = GisUtil.LayerIndex(aOutletGridFileName)
        Dim lOutletGrid As MapWinGIS.Grid = GridFromIndex(lOutletGridLayerIndex)
        Dim lFlowDirGridLayerIndex As Integer = GisUtil.LayerIndex(aFlowDirGridFileName)
        Dim lFlowDirGrid As MapWinGIS.Grid = GridFromIndex(lFlowDirGridLayerIndex)
        Dim lSubbasinGridLayerIndex As Integer = GisUtil.LayerIndex(aSubbasinGridFileName)
        Dim lSubbasinGrid As MapWinGIS.Grid = GridFromIndex(lSubbasinGridLayerIndex)

        Dim lStartRow As Integer = 0
        Dim lStartCol As Integer = 0
        Dim lEndRow As Integer = lOutletGrid.Header.NumberRows - 1
        Dim lEndCol As Integer = lOutletGrid.Header.NumberCols - 1

        'this is the assumed flow dir coding from taudem:
        '4 3 2
        '5   1
        '6 7 8

        Dim lOutletVal As Integer = 0
        Dim lFlowDirVal As Integer = 0
        Dim lDownSubRow As Integer = -1
        Dim lDownSubCol As Integer = -1
        Dim lDownstreamVal As Integer = lSubbasinGrid.Header.NodataValue
        Dim lDownstreamIDs As New atcCollection
        For lRow As Integer = lStartRow To lEndRow
            For lCol As Integer = lStartCol To lEndCol
                lOutletVal = lOutletGrid.Value(lCol, lRow)
                If lOutletVal > 0 Then
                    If lOutletVal = 217 Then
                        Logger.Dbg("at outlet")
                    End If
                    'found an outlet, see what is downstream of this
                    lFlowDirVal = lFlowDirGrid.Value(lCol, lRow)
                    If lFlowDirVal = 2 Or lFlowDirVal = 3 Or lFlowDirVal = 4 Then
                        lDownSubRow = lRow - 1
                    ElseIf lFlowDirVal = 6 Or lFlowDirVal = 7 Or lFlowDirVal = 8 Then
                        lDownSubRow = lRow + 1
                    Else
                        lDownSubRow = lRow
                    End If
                    If lFlowDirVal = 4 Or lFlowDirVal = 5 Or lFlowDirVal = 6 Then
                        lDownSubCol = lCol - 1
                    ElseIf lFlowDirVal = 2 Or lFlowDirVal = 1 Or lFlowDirVal = 8 Then
                        lDownSubCol = lCol + 1
                    Else
                        lDownSubCol = lCol
                    End If
                    If lDownSubCol >= lStartCol And lDownSubCol <= lEndCol And _
                       lDownSubRow >= lStartRow And lDownSubRow <= lEndRow Then
                        lDownstreamVal = lSubbasinGrid.Value(lDownSubCol, lDownSubRow)
                    End If
                    'in output grid we'll need to change from loutletval to ldownstreamval
                    lDownstreamIDs.Add(lOutletVal, lDownstreamVal)
                End If
            Next
        Next

        lOutletGrid = Nothing
        lSubbasinGrid = Nothing
        lFlowDirGrid = Nothing

        If FileExists(aDownstreamGridFileName) Then
            IO.File.Delete(aDownstreamGridFileName)
        End If
        IO.File.Copy(aSubbasinGridFileName, aDownstreamGridFileName)
        Dim lOutputGrid As New MapWinGIS.Grid
        lOutputGrid.Open(aDownstreamGridFileName)

        lStartRow = 0
        lStartCol = 0
        lEndRow = lOutputGrid.Header.NumberRows - 1
        lEndCol = lOutputGrid.Header.NumberCols - 1

        Dim lTempVal As Integer = 0
        For lRow As Integer = lStartRow To lEndRow
            For lCol As Integer = lStartCol To lEndCol
                lTempVal = lOutputGrid.Value(lCol, lRow)
                If lTempVal > 0 Then
                    'change this cell from loutletval to ldownstreamval
                    lOutputGrid.Value(lCol, lRow) = lDownstreamIDs.ItemByKey(lTempVal)
                End If
            Next
        Next

        lOutputGrid.Save()
        lOutputGrid = Nothing
    End Sub

    Public Shared Sub GridComputeVelocity(ByVal aVelocityGridFileName As String, ByVal aLuFileName As String, ByVal aSubbasinFileName As String, _
                                          ByVal aFlowAccFileName As String, ByVal aManningsValues As atcCollection, ByVal aSlopeBySubbasin As atcCollection)
        'compute flow velocity for GeoSFM, using flow acc, mannings value and slope
        'at each grid cell we know the land use code -> mannings value
        '                              subbasin id   -> slope
        '                              flow accumulation 

        Dim lVelGrid As New MapWinGIS.Grid
        lVelGrid.Open(aVelocityGridFileName)

        Dim lFlowAccGrid As New MapWinGIS.Grid
        lFlowAccGrid.Open(aFlowAccFileName)

        Dim lLuGrid As New MapWinGIS.Grid
        lLuGrid.Open(aLuFileName)

        Dim lSubbasinGrid As New MapWinGIS.Grid
        lSubbasinGrid.Open(aSubbasinFileName)

        Dim lStartRow As Integer = 0
        Dim lStartCol As Integer = 0
        Dim lEndRow As Integer = lFlowAccGrid.Header.NumberRows - 1
        Dim lEndCol As Integer = lFlowAccGrid.Header.NumberCols - 1
        Dim lFlowacc As Single = 0.0
        Dim lLuCode As Integer = 0

        Dim lX As Double = 0.0
        Dim lY As Double = 0.0
        Dim lLuCol As Integer = 0
        Dim lLuRow As Integer = 0
        Dim lManningsN As Single = 0.05
        Dim lSubid As Integer = 0
        Dim lSlope As Single = 0.0
        Dim lMsgDisplayed As Boolean = False
        Dim lVelocity As Single = 0.0
        For lRow As Integer = lStartRow To lEndRow
            For lCol As Integer = lStartCol To lEndCol
                'flow accumulation value at this point
                lFlowacc = lFlowAccGrid.Value(lCol, lRow)
                'get mannings n at this point from land use code
                lFlowAccGrid.CellToProj(lCol, lRow, lX, lY)
                lLuGrid.ProjToCell(lX, lY, lLuCol, lLuRow)
                lLuCode = lLuGrid.Value(lLuCol, lLuRow)
                If aManningsValues.Keys.Contains(lLuCode) Then
                    lManningsN = aManningsValues.ItemByKey(lLuCode)
                Else
                    If lMsgDisplayed = False Then
                        Logger.Msg("Anderson Code of " & lLuCode.ToString & " is not supported in the USGS Land Cover Grid." & vbCrLf & "Defaulting to Mannings n of 0.05", "Geospatial Stream Flow Model")
                        lMsgDisplayed = True
                    End If
                    lManningsN = 0.05
                End If
                'get slope at this point based on subbasin
                lSubid = lSubbasinGrid.Value(lCol, lRow)
                If aSlopeBySubbasin.Keys.Contains(lSubid) Then
                    lSlope = aSlopeBySubbasin.ItemByKey(lSubid)
                Else
                    lSlope = -1.0
                End If
                'now compute velocity
                If lSlope > -1.0 Then
                    If lFlowacc <= 1000 Then
                        'lVelocity = (0.015874 / lManningsN) * (lSlope ^ 0.5)
                        lVelocity = (0.1357209 / lManningsN) * (lSlope ^ 0.5)
                    ElseIf lFlowacc <= 2000 Then
                        'lVelocity = (0.0736806 / lManningsN) * (lSlope ^ 0.5)
                        lVelocity = (0.0736806 / lManningsN) * (lSlope ^ 0.5)
                    ElseIf lFlowacc <= 3000 Then
                        'lVelocity = (0.0464159 / lManningsN) * (lSlope ^ 0.5)
                        lVelocity = (0.0464159 / lManningsN) * (lSlope ^ 0.5)
                    ElseIf lFlowacc <= 4000 Then
                        'lVelocity = (0.0736806 / lManningsN) * (lSlope ^ 0.5)
                        lVelocity = (0.0292402 / lManningsN) * (lSlope ^ 0.5)
                    ElseIf lFlowacc <= 5000 Then
                        'lVelocity = (0.1357209 / lManningsN) * (lSlope ^ 0.5)
                        lVelocity = (0.015874 / lManningsN) * (lSlope ^ 0.5)
                    ElseIf lFlowacc <= 10000 Then
                        lVelocity = 0.3
                    ElseIf lFlowacc <= 50000 Then
                        lVelocity = 0.45
                    ElseIf lFlowacc <= 100000 Then
                        lVelocity = 0.6
                    ElseIf lFlowacc <= 250000 Then
                        lVelocity = 0.75
                    ElseIf lFlowacc <= 500000 Then
                        lVelocity = 0.9
                    ElseIf lFlowacc <= 750000 Then
                        lVelocity = 1.2
                    Else
                        lVelocity = 1.5
                    End If
                    If lVelocity > 1.5 Then
                        lVelocity = 1.5
                    End If
                    If lVelocity < 0.01 Then
                        lVelocity = 0.01
                    End If
                    lVelGrid.Value(lCol, lRow) = lVelocity
                Else
                    lVelGrid.Value(lCol, lRow) = 0.0
                End If
            Next
        Next

        lVelGrid.Save()
        lVelGrid.Close()
        lFlowAccGrid.Close()
        lSubbasinGrid.Close()
        lLuGrid.Close()

        lVelGrid = Nothing
        lFlowAccGrid = Nothing
        lSubbasinGrid = Nothing
        lLuGrid = Nothing
    End Sub

    Public Shared Function GridGetCellSizeX(ByVal aGridLayerIndex As Integer) As Double
        Dim lGrid As MapWinGIS.Grid = GridFromIndex(aGridLayerIndex)
        Return lGrid.Header.dX
    End Function

    Public Shared Function GridGetCellSizeY(ByVal aGridLayerIndex As String) As Double
        Dim lGrid As MapWinGIS.Grid = GridFromIndex(aGridLayerIndex)
        Return lGrid.Header.dY
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
                        If Not lShapeNew Is Nothing AndAlso lShapeNew.numPoints > 0 Then 'Insert the shape into the shapefile 
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
            If pStatusShow Then Logger.Progress("Overlay of Layer '" & lLayer1.Name & "' with Layer '" & lLayer2.Name & "'", lPolygonCount, lTotalPolygonCount)
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

    Public Shared Sub OverlaySelected(ByVal aLayer1Index As Integer, ByVal aLayer1SelectedIndex As Integer, _
                                      ByVal aLayer2Index As Integer, ByVal aLayer2SelectedIndex As Integer, _
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

        Dim lBsuc As Boolean
        Dim lSfOut As New MapWinGIS.Shapefile
        If aCreateNew Then 'create new output overlay shapefile
            lSfOut.CreateNew("overlay", MapWinGIS.ShpfileType.SHP_POLYGON)
            Dim [lOf] As New MapWinGIS.Field
            [lOf].Name = "loverlay"
            [lOf].Type = MapWinGIS.FieldType.INTEGER_FIELD
            [lOf].Width = 10
            lBsuc = lSfOut.EditInsertField([lOf], 0)
        Else 'open existing output shape file
            lBsuc = lSfOut.Open(aOutputLayerName)
        End If

        lSfOut.StartEditingShapes(True)

        Dim lShapeNew As MapWinGIS.Shape
        Dim lShape1 As MapWinGIS.Shape
        Dim lShape1Ext As MapWinGIS.Extents
        Dim lShape2 As MapWinGIS.Shape
        Dim lShape2Ext As MapWinGIS.Extents

        lShape2 = lSf2.Shape(aLayer2SelectedIndex)
        lShape1 = lSf1.Shape(aLayer1SelectedIndex)
        lShape2Ext = lShape2.Extents
        lShape1Ext = lShape1.Extents

        '********** do overlay ***********

        If Not (lShape1Ext.xMin > lShape2Ext.xMax OrElse _
                lShape1Ext.xMax < lShape2Ext.xMin OrElse _
                lShape1Ext.yMin > lShape2Ext.yMax OrElse _
                lShape1Ext.yMax < lShape2Ext.yMin) Then
            'look for intersection from overlay of these shapes
            lShapeNew = MapWinGeoProc.SpatialOperations.Intersection(lShape1, lShape2)
            If Not lShapeNew Is Nothing AndAlso lShapeNew.numPoints > 0 Then 'Insert the shape into the shapefile 
                lBsuc = lSfOut.EditInsertShape(lShapeNew, lSfOut.NumShapes)
                If Not lBsuc Then
                    Logger.Dbg("Problem Adding Shape") 'TODO:add more details, message box?
                End If
            End If
            lShapeNew = Nothing
        End If

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
                        If Not lShapeNew Is Nothing AndAlso lShapeNew.numPoints > 0 Then 'Insert the shape into the shapefile 
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

            If pStatusShow Then Logger.Progress("Tabulating Polygon Areas...", lPolygonCount, lTotalPolygonCount)
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
                If pStatusShow Then Logger.Progress("Clipping Shapes with Polygon...", lCount, lTotal)
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
            If pStatusShow Then Logger.Progress("Merging Features...", i, lsf.NumShapes)
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
                If pStatusShow Then Logger.Progress("Merging Features...", i, lsf.NumShapes)
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
        Return True
    End Function

    ''' <summary>
    ''' Merge all selected shapes in one shape file and append merged shape to destination shape file
    ''' </summary>
    ''' <param name="aLayerIndexSource">Index of source layer having selected shaped</param>
    ''' <param name="aLayerIndexDest">Index of destination layer where merged shape will be added</param>
    ''' <returns></returns>
    ''' <remarks>Programmed by LCW for GBMM project</remarks>
    Public Shared Function MergeSelectedShapes(ByVal aLayerIndexSource As Integer, ByVal aLayerIndexDest As Integer) As Boolean
        Try
            'build collection of selected shape indexes
            Dim lSelectedShapeIndexes As New atcCollection
            For lIndex As Integer = 1 To NumSelectedFeatures(aLayerIndexSource)
                lSelectedShapeIndexes.Add(IndexOfNthSelectedFeatureInLayer(lIndex - 1, aLayerIndexSource))
            Next
            lSelectedShapeIndexes.Sort()
            If lSelectedShapeIndexes.Count = 0 Then Return False

            'copy source file to temp file
            Dim tempfile As String = My.Computer.FileSystem.SpecialDirectories.Temp & "\tempfile.shp"

            If Not MapWinGeoProc.DataManagement.CopyShapefile(LayerFileName(aLayerIndexSource), tempfile) Then Return False

            Dim mergeShp As MapWinGIS.Shape

            Dim sfSource As New MapWinGIS.Shapefile

            With sfSource
                If Not .Open(tempfile) Then Return False
                If Not .StartEditingShapes(True) Then Return False

                'insert first shape at end
                If Not .EditInsertShape(.Shape(lSelectedShapeIndexes(0)), .NumShapes) Then Return False

                For i As Integer = 1 To lSelectedShapeIndexes.Count - 1
                    Dim newShp As New MapWinGIS.Shape
                    If Not newShp.Create(.ShapefileType) Then Return False
                    If Not MapWinGeoProc.SpatialOperations.MergeShapes(sfSource, lSelectedShapeIndexes(i), .NumShapes - 1, newShp) Then Return False
                    'replace last shape with this one
                    If Not .EditDeleteShape(.NumShapes - 1) Then Return False
                    If Not .EditInsertShape(newShp, .NumShapes) Then Return False
                    If pStatusShow Then Logger.Progress("Merging shapes...", i, lSelectedShapeIndexes.Count - 1)
                    System.Windows.Forms.Application.DoEvents()
                Next

                'now all selected have been merged into last shape; save it for later
                mergeShp = .Shape(.NumShapes - 1)

                If Not .StopEditingShapes(False, True) Then Return False
                If Not .Close() Then Return False
                My.Computer.FileSystem.DeleteFile(tempfile)
            End With

            If pStatusShow Then Logger.Status("Appending merged shape...")
            Dim sfDest As New MapWinGIS.Shapefile
            With sfDest
                If Not .Open(LayerFileName(aLayerIndexDest)) Then Return False
                If Not .StartEditingShapes(True) Then Return False
                If Not .EditInsertShape(mergeShp, .NumShapes) Then Return False
                If Not .StopEditingShapes(True, True) Then Return False
                If Not .Close Then Return False
            End With

            pMapWin.View.Redraw()

            ClearSelectedFeatures(aLayerIndexSource)

            'for some reason, need to unload then reload destination layer (?)
            Dim lyrName As String = LayerName(aLayerIndexDest)
            Dim lyrGrp As String = LayerGroup(aLayerIndexDest)
            Dim lyrFile As String = LayerFileName(aLayerIndexDest)

            RemoveLayer(aLayerIndexDest)

            If Not AddLayerToGroup(lyrFile, lyrName, lyrGrp) Then Return False

            If Not SetSelectedFeature(LayerIndex(lyrName), NumFeatures(aLayerIndexDest) - 1) Then Return False
            Return True
        Catch ex As Exception
            Throw ex
        Finally
            Logger.Progress("", 100, 100) 'clear the progressbar
        End Try
    End Function

    ''' <summary>
    ''' Remove all vertices that are closer than specified distance
    ''' </summary>
    ''' <param name="shp">Polygon shape to be filtered</param>
    ''' <param name="FilterSize">Minimum distance between adjacent points</param>
    ''' <returns>True if successful</returns>
    ''' <remarks>Programmed by LCW for GBMM project</remarks>
    <CLSCompliant(False)> _
    Public Shared Function FilterShape(ByRef shp As MapWinGIS.Shape, ByVal FilterSize As Single) As Boolean
        Try
            With shp
                Dim lstPoints As New Generic.List(Of MapWinGIS.Point)
                lstPoints.Add(.Point(0))
                Dim lastPt As MapWinGIS.Point = .Point(0)
                For i As Integer = 1 To .numPoints - 2
                    With .Point(i)
                        Dim dist As Single = Math.Sqrt((.x - lastPt.x) ^ 2 + (.y - lastPt.y) ^ 2)
                        If dist > FilterSize Then
                            lstPoints.Add(shp.Point(i))
                            lastPt = shp.Point(i)
                        End If
                    End With
                Next
                lstPoints.Add(.Point(.numPoints - 1))
                Dim shpNew As New MapWinGIS.Shape
                shpNew.Create(.ShapeType)
                For i As Integer = 0 To lstPoints.Count - 1
                    shpNew.InsertPoint(lstPoints(i), i)
                Next
                shp = shpNew
            End With
            Return True

        Catch ex As Exception
            Throw ex
            Return False
        End Try
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
        If Not lNewShape Is Nothing AndAlso lNewShape.numPoints > 0 Then
            lAreaOverlappingPolygons = Math.Abs(MapWinGeoProc.Utils.Area(lNewShape))
        End If
        lNewShape = Nothing
        Return lAreaOverlappingPolygons
    End Function

    Public Shared Sub SaveSelectedFeatures(ByVal aAreaLayerIndex As Integer, _
                                    ByVal aSelectedAreaIndexes As Collection, _
                                    ByRef aNewFilename As String, Optional ByVal aType As String = "same")

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

        If aType = "polylinez" Then
            lStatus = tollSF.CreateNew(aNewFilename, MapWinGIS.ShpfileType.SHP_POLYLINEZ)
        Else
            If lLayer.LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                lStatus = tollSF.CreateNew(aNewFilename, MapWinGIS.ShpfileType.SHP_POLYGON)
            ElseIf lLayer.LayerType = MapWindow.Interfaces.eLayerType.LineShapefile Then
                lStatus = tollSF.CreateNew(aNewFilename, MapWinGIS.ShpfileType.SHP_POLYLINE)
            ElseIf lLayer.LayerType = MapWindow.Interfaces.eLayerType.PointShapefile Then
                lStatus = tollSF.CreateNew(aNewFilename, MapWinGIS.ShpfileType.SHP_POINT)
            End If
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

    Public Shared Sub SetLayerRendererWithRanges(ByVal aLayerIndex As Integer, ByVal aFieldIndex As Integer, ByVal aColors As Collection, _
                                                 ByVal aCaptions As Collection, ByVal aLowRange As Collection, ByVal aHighRange As Collection)
        'create a unique values renderer for the given layer and field,
        'adapted from MapWindow SFColoringSchemeForm.vb

        Dim lColorScheme As New MapWinGIS.ShapefileColorScheme

        Dim lLayer As MapWindow.Interfaces.Layer = LayerFromIndex(aLayerIndex)

        Dim lSf As New MapWinGIS.Shapefile
        lSf = lLayer.GetObject()

        lColorScheme.FieldIndex = aFieldIndex
        For i As Integer = 1 To aColors.Count
            Dim lBrk As New MapWinGIS.ShapefileColorBreak
            lBrk.StartColor = aColors(i)
            lBrk.EndColor = lBrk.StartColor
            lBrk.StartValue = aLowRange(i)
            lBrk.EndValue = aHighRange(i)
            lBrk.Caption = aCaptions(i)
            lColorScheme.Add(lBrk)
            lBrk = Nothing
        Next

        lSf = Nothing

        lLayer.ColoringScheme = lColorScheme
        lLayer.Expanded = True
        lLayer.Visible = True
    End Sub

    Public Shared Sub SetLayerRendererUniqueValues(ByVal aDesc As String, ByVal aFieldIndex As Integer, Optional ByVal aColors As Collection = Nothing, _
                                                   Optional ByVal aCaptions As Collection = Nothing)
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

            If Not aColors Is Nothing Then
                If aColors.Count > i Then
                    lBrk.StartColor = aColors(i + 1)
                    lBrk.EndColor = lBrk.StartColor
                End If
            End If
            If Not aCaptions Is Nothing Then
                If aCaptions.Count > i Then
                    lBrk.Caption = aCaptions(i + 1)
                End If
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

    Public Shared Sub SetLayerLineSize(ByVal aLayerIndex As String, ByVal aLineSize As Integer)
        Dim lLayer As MapWindow.Interfaces.Layer = LayerFromIndex(aLayerIndex)
        lLayer.LineOrPointSize = aLineSize
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

    Public Shared Function CreateEmptyShapefile(ByVal aShapefileName As String, _
                                                ByVal aProjection As String, ByVal aType As String) As Boolean
        Dim lShapefile As New MapWinGIS.Shapefile

        Dim lMWType As MapWinGIS.ShpfileType
        If aType.ToLower = "polygon" Then
            lMWType = MapWinGIS.ShpfileType.SHP_POLYGON
        ElseIf aType.ToLower = "line" Then
            lMWType = MapWinGIS.ShpfileType.SHP_POLYLINE
        ElseIf aType.ToLower = "point" Then
            lMWType = MapWinGIS.ShpfileType.SHP_POINT
        End If

        If Not lShapefile.CreateNew(aShapefileName, lMWType) Then
            Logger.Dbg("Failed to create new shapefile " & aShapefileName & " ErrorCode " & lShapefile.LastErrorCode)
            Return False
        End If

        If aProjection.Length > 0 Then
            lShapefile.Projection = aProjection
        Else
            lShapefile.Projection = GisUtil.ProjectProjection
        End If
        If Not lShapefile.SaveAs(lShapefile.Filename) Then
            Return False
        End If
        If Not lShapefile.Close() Then
            Logger.Dbg("Failed to close shapefile " & lShapefile.Filename & " ErrorCode " & lShapefile.LastErrorCode)
            Return False
        End If
        Return True

    End Function

    Public Shared Sub ShapeCentroid(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer, ByRef aCentroidX As Double, ByRef aCentroidY As Double)
        Dim lSf As New MapWinGIS.Shapefile
        lSf = ShapeFileFromIndex(aLayerIndex)
        Dim lPt As New MapWinGIS.Point
        lPt = MapWinGeoProc.Utils.Centroid(lSf.Shape(aFeatureIndex))
        aCentroidX = lPt.x
        aCentroidY = lPt.y
    End Sub

    Public Shared Sub AddCentroidXYtoShapefile(ByVal aShapefileName As String)
        Dim lSf As New MapWinGIS.Shapefile
        lSf.Open(aShapefileName)

        Dim lFieldIndex As Integer
        Dim lXField As Integer = -1
        For lFieldIndex = 0 To lSf.NumFields - 1
            If lSf.Field(lFieldIndex).Name.ToUpper = "CentroidX".ToUpper Then 'this is the field we want
                lXField = lFieldIndex
            End If
        Next
        If lXField < 0 Then
            Dim lField As New MapWinGIS.Field
            lField.Name = "CentroidX"
            lField.Type = MapWinGIS.FieldType.DOUBLE_FIELD
            lField.Width = 10
            lSf.StartEditingTable()
            lFieldIndex = lSf.NumFields
            Dim lBsuc As Boolean = lSf.EditInsertField(lField, lFieldIndex)
            lXField = lFieldIndex
            lSf.StopEditingTable()
        End If

        Dim lYField As Integer = -1
        For lFieldIndex = 0 To lSf.NumFields - 1
            If lSf.Field(lFieldIndex).Name.ToUpper = "CentroidY".ToUpper Then 'this is the field we want
                lYField = lFieldIndex
            End If
        Next
        If lYField < 0 Then
            Dim lField As New MapWinGIS.Field
            lField.Name = "CentroidY"
            lField.Type = MapWinGIS.FieldType.DOUBLE_FIELD
            lField.Width = 10
            lSf.StartEditingTable()
            lFieldIndex = lSf.NumFields
            Dim lBsuc As Boolean = lSf.EditInsertField(lField, lFieldIndex)
            lYField = lFieldIndex
            lSf.StopEditingTable()
        End If

        Dim lPt As New MapWinGIS.Point
        lSf.StartEditingTable()
        For lShapeIndex As Integer = 0 To lSf.NumShapes - 1
            lPt = MapWinGeoProc.Utils.Centroid(lSf.Shape(lShapeIndex))
            lSf.EditCellValue(lXField, lShapeIndex, lPt.x)
            lSf.EditCellValue(lYField, lShapeIndex, lPt.y)
        Next
        lSf.StopEditingTable()
    End Sub

    Public Shared Sub LineCentroid(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer, ByRef aCentroidX As Double, ByRef aCentroidY As Double)
        Dim lSf As New MapWinGIS.Shapefile
        lSf = ShapeFileFromIndex(aLayerIndex)
        Dim lShape As MapWinGIS.Shape = lSf.Shape(aFeatureIndex)
        aCentroidX = ((lShape.Extents.xMax - lShape.Extents.xMin) / 2) + lShape.Extents.xMin
        aCentroidY = ((lShape.Extents.yMax - lShape.Extents.yMin) / 2) + lShape.Extents.yMin
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

    ''' <summary>
    ''' Create unique color scheme for shapefile layer
    ''' </summary>
    ''' <param name="aLayerIndex">Index of desired layer</param>
    ''' <param name="aFieldIndex">Index of desired field</param>
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

    Public Shared Sub BufferLayer(ByVal aInputShapefileFilename As String, ByVal aResultShapefileFilename As String, _
                                  ByVal aBufferDistance As Double)
        'create a new shapefile as a buffer around the specified layer
        'MapWinGeoProc.SpatialOperations.BufferSF(aInputShapefileFilename, aResultShapefileFilename, aBufferDistance, False)

        'Can replace geoproc version with one that uses ocx, seems to be more reliable 
        Dim lInSf As New MapWinGIS.Shapefile
        lInSf.Open(aInputShapefileFilename)
        Dim lOutSf As New MapWinGIS.Shapefile
        If lOutSf.CreateNew(aResultShapefileFilename, MapWinGIS.ShpfileType.SHP_POLYGON) Then
            For lFieldIndex As Integer = 1 To lInSf.NumFields
                lOutSf.EditInsertField(lInSf.Field(lFieldIndex - 1), lFieldIndex - 1)
            Next lFieldIndex
            lOutSf.StartEditingShapes()
            For lIndex As Integer = 0 To lInSf.NumShapes - 1
                Dim lShape As MapWinGIS.Shape = lInSf.Shape(lIndex)
                Dim lNewShape As New MapWinGIS.Shape
                lNewShape = lShape.Buffer(aBufferDistance, 0)
                lOutSf.EditInsertShape(lNewShape, lOutSf.NumShapes)
                For lFieldIndex As Integer = 0 To lInSf.NumFields - 1
                    lOutSf.EditCellValue(lFieldIndex, lOutSf.NumShapes - 1, lInSf.CellValue(lFieldIndex, lIndex))
                Next lFieldIndex
            Next
        End If
        lOutSf.SaveAs(aResultShapefileFilename)
        lOutSf.Close()

        Dim lInputProjectionFileName As String = FilenameSetExt(aInputShapefileFilename, "prj")
        If FileExists(lInputProjectionFileName) Then
            FileCopy(lInputProjectionFileName, FilenameSetExt(aResultShapefileFilename, "prj"))
        End If
    End Sub

    Public Shared Function NearestNeighbor(ByVal aX As Double, ByVal aY As Double, ByVal aPolygonLayerIndex As Integer) As Integer
        'given a point and a polygon shapefile, find index of closest neighboring polygon to this point

        'this is accomplished by finding the point on the containing polygon's boarder closest to the input point,
        'and then using the selection functionality to see which other polygon coincides with that point.

        Dim lPt As New MapWinGIS.Point
        lPt.x = aX
        lPt.y = aY

        'is this point within a polygon?
        Dim lPolygonFeatureIndex As Integer = PointInPolygonXY(aX, aY, aPolygonLayerIndex)
        If lPolygonFeatureIndex > -1 Then
            'yes
            Dim lPolygonShapefile As MapWinGIS.Shapefile = ShapeFileFromIndex(aPolygonLayerIndex)
            Dim lOutputPoint As New MapWinGIS.Point
            If FeatureIndexValid(lPolygonFeatureIndex, lPolygonShapefile) Then
                Dim lContainingShape As MapWinGIS.Shape = lPolygonShapefile.Shape(lPolygonFeatureIndex)
                Dim lLocation As Integer
                Dim lDistance As Double
                MapWinGeoProc.Utils.FindNearestPointAndLoc(lPt, lContainingShape, lOutputPoint, lLocation, lDistance)
            End If
            If lOutputPoint.x <> 0 And lOutputPoint.y <> 0 Then
                'found a point on the boarder
                Dim lExtents As New MapWinGIS.Extents
                lExtents.SetBounds(lOutputPoint.x, lOutputPoint.y, 0, lOutputPoint.x, lOutputPoint.y, 0)
                Dim lIntersectingPolygonIndices As Object = Nothing
                lPolygonShapefile.SelectShapes(lExtents, 0.0, MapWinGIS.SelectMode.INTERSECTION, lIntersectingPolygonIndices)
                'could use MapWinGeoProc.Selection.SelectPolygonsWithPolygon instead?
                If lIntersectingPolygonIndices.GetUpperBound(0) > 0 Then
                    'found some intersecting polygons
                    For lIndex As Integer = 0 To lIntersectingPolygonIndices.GetUpperBound(0) - 1
                        If lIntersectingPolygonIndices(lIndex) <> lPolygonFeatureIndex Then
                            'this polygon is not the one the input point is in, so it must be the nearest neighbor
                            NearestNeighbor = lIntersectingPolygonIndices(lIndex)
                        End If
                    Next
                End If
            End If
        Else
            'no, we have to be within a polygon to find the nearest neighbor
            NearestNeighbor = -1
        End If
    End Function

    Public Shared Sub CopyAllAttributes(ByVal aSourceLayerIndex As Integer, ByVal aSourceFeatureIndex As Integer, ByVal aTargetLayerIndex As Integer, ByVal aTargetFeatureIndex As Integer)
        'copy all attributes from a source feature to a target feature
        Dim lSourceSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aSourceLayerIndex)
        Dim lTargetSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aTargetLayerIndex)
        If Not FeatureIndexValid(aSourceFeatureIndex, lSourceSf) Then
            Throw New Exception("GisUtil:FieldValue:Error:FeatureIndex:" & aSourceFeatureIndex & ":OutOfRange")
        ElseIf Not FeatureIndexValid(aTargetFeatureIndex, lTargetSf) Then
            Throw New Exception("GisUtil:FieldValue:Error:FeatureIndex:" & aTargetFeatureIndex & ":OutOfRange")
        Else
            For lSourceFieldIndex As Integer = 0 To lSourceSf.NumFields - 1
                Dim lFieldName As String = lSourceSf.Field(lSourceFieldIndex).Name
                If Not IsField(aTargetLayerIndex, lFieldName) Then
                    Dim lFieldWidth As Integer = lSourceSf.Field(lSourceFieldIndex).Width
                    Dim lFieldType As Integer = lSourceSf.Field(lSourceFieldIndex).Type
                    GisUtil.AddField(aTargetLayerIndex, lFieldName, lFieldType, lFieldWidth)
                    GisUtil.StartSetFeatureValue(aTargetLayerIndex)
                End If
                Dim lTargetFieldIndex As Integer = FieldIndex(aTargetLayerIndex, lFieldName)
                SetFeatureValueNoStartStop(aTargetLayerIndex, lTargetFieldIndex, aTargetFeatureIndex, FieldValue(aSourceLayerIndex, aSourceFeatureIndex, lSourceFieldIndex))
            Next
        End If
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

    Public Shared Property DrawFill(Optional ByVal aLayerIndex As Integer = UseCurrent) As Boolean
        Get
            Return LayerFromIndex(aLayerIndex).DrawFill
        End Get
        Set(ByVal aDrawFill As Boolean)
            LayerFromIndex(aLayerIndex).DrawFill = aDrawFill
        End Set
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

#Region "Added by Chris Wilson during Sediment/Mercury WCS project"

    ''' <summary>
    ''' Get currently active project mapping units (e.g., "Feet")
    ''' </summary>
    ''' <remarks>Added by Chris Wilson 11/24/2008</remarks>
    Public Shared ReadOnly Property MapUnits() As String
        Get
            Return GetMappingObject.Project.MapUnits
        End Get
    End Property

    ''' <summary>Layer file name from a layer name</summary>
    ''' <param name="aLayerName">
    '''     <para>Name of layer</para>
    ''' </param>
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    ''' <remarks>Added by Chris Wilson 11/25/2008</remarks>
    Public Shared ReadOnly Property LayerFileName(ByVal aLayerName As String) As String
        Get
            Return LayerFromIndex(LayerIndex(aLayerName)).FileName
        End Get
    End Property

    ''' <summary>Layer type from a layer name</summary>
    ''' <param name="aLayerName">
    '''     <para>Name of layer.</para>
    ''' </param>
    ''' <exception cref="System.Exception" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
    ''' <exception cref="MappingObjectNotSetException">Mapping Object Not Set</exception>
    ''' <remarks>Added by Chris Wilson 11/25/2008</remarks>
    <CLSCompliant(False)> _
    Public Shared ReadOnly Property LayerType(ByVal aLayerName As String) As MapWindow.Interfaces.eLayerType
        Get
            Return LayerFromIndex(LayerIndex(aLayerName)).LayerType
        End Get
    End Property

    ''' <summary>
    ''' Zoom to the extents of the specified layer
    ''' </summary>
    ''' <param name="aLayerIndex">Index of layer.</param>
    ''' <remarks>Added by Chris Wilson 11/25/2008</remarks>
    Public Shared Sub ZoomToLayerExtents(ByVal aLayerIndex As Integer)
        LayerFromIndex(aLayerIndex).ZoomTo()
    End Sub

    ''' <summary>
    ''' Zoom to the extents of the specified layer name
    ''' </summary>
    ''' <param name="aLayerName">Name of layer.</param>
    ''' <remarks>Added by Chris Wilson 11/25/2008</remarks>
    Public Shared Sub ZoomToLayerExtents(ByVal aLayerName As String)
        LayerFromIndex(LayerIndex(aLayerName)).ZoomTo()
    End Sub

    ''' <summary>Given two polygon layers, calculate the area of each polygon of the 
    '''          first layer within each polygon of the second layer. 
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
    ''' <returns>Array (one item per Polygon2 shape) of dictionaries (key is Layer1 field value, value is total area)</returns>
    ''' <remarks>Added by Chris Wilson 11/24/2008 because original version required numeric entries in field</remarks>
    Public Shared Function TabulatePolygonAreas(ByVal aPolygonLayer1Index As Integer, _
                                                ByVal aLayer1FieldIndex As Integer, _
                                                ByVal aPolygonLayer2Index As Integer) As Generic.SortedDictionary(Of String, Double())

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
        Dim lSf2Shape As New Generic.List(Of MapWinGIS.Shape)
        Dim lSf2ShapeExtXmax As New Generic.List(Of Double)
        Dim lSf2ShapeExtXmin As New Generic.List(Of Double)
        Dim lSf2ShapeExtYmax As New Generic.List(Of Double)
        Dim lSf2ShapeExtYmin As New Generic.List(Of Double)
        For k As Integer = 0 To NumFeatures(aPolygonLayer2Index) - 1
            'loop thru each selected subbasin (or all if none selected)
            lShape2 = lSf2.Shape(k)
            lShape2Ext = lShape2.Extents
            lSf2Shape.Add(lShape2)
            lSf2ShapeExtXmax.Add(lShape2Ext.xMax)
            lSf2ShapeExtXmin.Add(lShape2Ext.xMin)
            lSf2ShapeExtYmax.Add(lShape2Ext.yMax)
            lSf2ShapeExtYmin.Add(lShape2Ext.yMin)
        Next k
        Dim lNumFeatures As Integer = GisUtil.NumFeatures(aPolygonLayer2Index)

        'set up collection of areas for each subbasin and land use type
        Dim lArea As New Generic.SortedDictionary(Of String, Double())

        '********** do overlay ***********

        Dim lNumShapes As Integer = lSf1.NumShapes
        For i As Integer = 1 To lNumShapes 'loop through each shape of the land use layer
            lShape1 = lSf1.Shape(i - 1)
            Dim lSf1Ext As MapWinGIS.Extents = lShape1.Extents
            If Not (lSf1Ext.xMin > lSf2Ext.xMax OrElse _
                    lSf1Ext.xMax < lSf2Ext.xMin OrElse _
                    lSf1Ext.yMin > lSf2Ext.yMax OrElse _
                    lSf1Ext.yMax < lSf2Ext.yMin) Then
                'current first polygon falls in the extents of the second shapefile
                For lShapeIndex As Integer = 0 To NumFeatures(aPolygonLayer2Index) - 1
                    'loop thru each shape in second shapefile
                    If Not (lSf1Ext.xMin > lSf2ShapeExtXmax(lShapeIndex) OrElse _
                            lSf1Ext.xMax < lSf2ShapeExtXmin(lShapeIndex) OrElse _
                            lSf1Ext.yMin > lSf2ShapeExtYmax(lShapeIndex) OrElse _
                            lSf1Ext.yMax < lSf2ShapeExtYmin(lShapeIndex)) Then
                        'look for intersection from overlay of these shapes
                        lShapeNew = MapWinGeoProc.SpatialOperations.Intersection(lShape1, lSf2Shape(lShapeIndex))
                        If lShapeNew.numPoints > 0 Then 'Insert the shape into the shapefile 
                            Dim lShapeArea As Double = Math.Abs(MapWinGeoProc.Utils.Area(lShapeNew))
                            Dim lFeature1Id As String = FieldValue(aPolygonLayer1Index, i - 1, aLayer1FieldIndex)
                            With lArea
                                Dim lAreaPrev() As Double = Nothing
                                If .TryGetValue(lFeature1Id, lAreaPrev) Then
                                    .Remove(lFeature1Id)
                                Else
                                    ReDim lAreaPrev(lNumFeatures - 1)
                                End If
                                lAreaPrev(lShapeIndex) += lShapeArea
                                .Add(lFeature1Id, lAreaPrev)
                            End With
                        End If
                        lShapeNew = Nothing
                    End If
                Next
            End If

            lSf1Ext = Nothing
            lShape1 = Nothing
        Next i
        Return lArea
    End Function

    ''' <summary>Given a grid and a polygon layer, calculate the area of each grid category 
    '''          within each polygon.  Output dictionary contains area of areas;
    '''          key is grid value, value is array of total areas (one for each shape in basin).</summary>
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
    '''     <para>Index of polygon layer to calculate areas for</para>
    ''' </param>
    ''' <returns>Sorted dictionary containing array of areas (key is grid value, value is array of total areas (one for each shape in basin))</returns>
    ''' <remarks>Added by Chris Wilson 11/24/2008 to make consistent with TabulatePolygonAreas</remarks>
    Public Shared Function TabulateAreas(ByVal aGridLayerIndex As Integer, _
                                         ByVal aPolygonLayerIndex As Integer) As Generic.SortedDictionary(Of String, Double())
        'set input grid
        Dim lInputGrid As MapWinGIS.Grid = GridFromIndex(aGridLayerIndex)
        'set input polygon layer
        Dim lPolygonSf As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aPolygonLayerIndex)

        Dim lMinX As Double = Double.MaxValue
        Dim lMaxX As Double = Double.MinValue
        Dim lMinY As Double = Double.MaxValue
        Dim lMaxY As Double = Double.MinValue

        For lPolyIndex As Integer = 0 To NumFeatures(aPolygonLayerIndex) - 1
            lMinX = Math.Min(lMinX, lPolygonSf.Shape(lPolyIndex).Extents.xMin)
            lMaxX = Math.Max(lMaxX, lPolygonSf.Shape(lPolyIndex).Extents.xMax)
            lMinY = Math.Min(lMinY, lPolygonSf.Shape(lPolyIndex).Extents.yMin)
            lMaxY = Math.Max(lMaxY, lPolygonSf.Shape(lPolyIndex).Extents.yMax)
        Next

        Dim lStartingColumn As Integer
        Dim lEndingColumn As Integer
        Dim lStartingRow As Integer
        Dim lEndingRow As Integer

        lInputGrid.ProjToCell(lMinX, lMinY, lStartingColumn, lEndingRow)
        lInputGrid.ProjToCell(lMaxX, lMaxY, lEndingColumn, lStartingRow)

        lStartingRow = Math.Max(0, lStartingRow)
        lEndingRow = Math.Min(lInputGrid.Header.NumberRows - 1, lEndingRow)
        lStartingColumn = Math.Max(0, lStartingColumn)
        lEndingColumn = Math.Min(lInputGrid.Header.NumberCols - 1, lEndingColumn)

        Dim lCellArea As Double = lInputGrid.Header.dX * lInputGrid.Header.dY

        'set up collection of areas for each subbasin and land use type
        Dim lArea As New Generic.SortedDictionary(Of String, Double())

        lPolygonSf.BeginPointInShapefile()
        Dim lXPos As Double
        Dim lYPos As Double
        Dim lShapeIndex As Integer
        Dim lGridValue As Integer
        Dim lNumFeatures As Integer = GisUtil.NumFeatures(aPolygonLayerIndex)

        For lRow As Integer = lStartingRow To lEndingRow
            For lCol As Integer = lStartingColumn To lEndingColumn
                lInputGrid.CellToProj(lCol, lRow, lXPos, lYPos)
                lShapeIndex = lPolygonSf.PointInShapefile(lXPos, lYPos)
                If lShapeIndex > -1 Then 'this is in a subbasin
                    If lInputGrid.Value(lCol, lRow).GetType.Name = "SByte" Then
                        lGridValue = Convert.ToInt32(lInputGrid.Value(lCol, lRow))
                    Else
                        lGridValue = lInputGrid.Value(lCol, lRow)
                    End If

                    With lArea
                        Dim lAreaPrev() As Double = Nothing
                        If .TryGetValue(lGridValue, lAreaPrev) Then
                            .Remove(lGridValue)
                        Else
                            ReDim lAreaPrev(lNumFeatures - 1)
                        End If
                        lAreaPrev(lShapeIndex) += lCellArea
                        .Add(lGridValue, lAreaPrev)
                    End With
                End If
            Next lCol
        Next lRow
        lPolygonSf.EndPointInShapefile()
        Return lArea
    End Function

    ''' <summary>
    ''' Can be used to cancel long-running calcs
    ''' </summary>
    ''' <remarks>Caller can monitor some event and set this true to cancel routine</remarks>
    Public Shared Cancel As Boolean

    ''' <summary>
    ''' Can be used to report progress in long-running routine when logger.progress is not enough
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Event Progress(ByVal Message As String, ByVal ItemNum As Integer, ByVal MaxItemNum As Integer)

    ''' <summary>Given a shapefile (landuse) and polygon (subbasin) layers, calculate the area of each landuse category 
    '''          within each subbasin polygon.</summary>
    ''' <remark>This function can be accomplished in MapWindow by 
    '''         looping through each grid cell and counting the 
    '''         number of cells of each grid category within each 
    '''         feature. The MapWinGIS calls to use include the Grid 
    '''         Property Value, the GridHeader Properties dX, dY, XllCenter, 
    '''         and YllCenter, and the Shapefile Function PointInShapefile.</remark>
    ''' <param name="SubbasinLayerName">Name of shapefile (e.g., subbasins)</param>
    ''' <param name="SubbasinFieldName">Name of field containing subbasin ID or Name</param>
    ''' <param name="LanduseLayerName">Name of grid file (e.g., landuse)</param>
    ''' <returns>Output is dictionary (key is polygon ID) of dictionaries (key is grid value, value is area).</returns>
    ''' <remarks>Added by Chris Wilson 5/24/2009</remarks>
    Public Shared Function TabulateAreas(ByVal SubbasinLayerName As String, ByVal SubbasinFieldName As String, _
                                         ByVal LanduseLayerName As String, ByVal LanduseFieldName As String) _
                                         As Generic.SortedDictionary(Of String, Generic.SortedDictionary(Of String, Single))
        Cancel = False
        Dim sfSubbasin As MapWinGIS.Shapefile = Nothing, sfLanduse As MapWinGIS.Shapefile = Nothing
        Try
            If Not (IsLayer(LanduseLayerName) AndAlso IsField(LayerIndex(LanduseLayerName), LanduseFieldName) AndAlso IsLayer(SubbasinLayerName) AndAlso IsField(LayerIndex(SubbasinLayerName), SubbasinFieldName)) Then Throw New Exception("Layer or field does not exist.")

            sfSubbasin = New MapWinGIS.Shapefile
            If Not sfSubbasin.Open(LayerFileName(SubbasinLayerName)) Then Throw New Exception("Unable to open layer: " & SubbasinLayerName)
            Dim fldSubbasin As Integer = FieldIndex(LayerIndex(SubbasinLayerName), SubbasinFieldName)

            sfLanduse = New MapWinGIS.Shapefile
            If Not sfLanduse.Open(LayerFileName(LanduseLayerName)) Then Throw New Exception("Unable to open layer: " & LanduseLayerName)
            Dim fldLanduse As Integer = FieldIndex(LayerIndex(LanduseLayerName), LanduseFieldName)

            Dim dictSubbasin As New Generic.SortedDictionary(Of String, Generic.SortedDictionary(Of String, Single))

            For i As Integer = 0 To sfSubbasin.NumShapes - 1
                Dim NumErrors As Integer = 0
                Dim shpSubbasin As MapWinGIS.Shape = sfSubbasin.Shape(i)
                Dim extSB As MapWinGIS.Extents = sfSubbasin.QuickExtents(i)
                Dim ID As String = sfSubbasin.CellValue(fldSubbasin, i)
                If dictSubbasin.ContainsKey(ID) Then
                    Logger.Message(String.Format("Subbasin name field ({0}) contains duplicate values.", SubbasinFieldName), "Error", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
                    Return Nothing
                End If
                dictSubbasin.Add(ID, New Generic.SortedDictionary(Of String, Single))
                Dim jmax As Integer = sfLanduse.NumShapes - 1
                For j As Integer = 0 To jmax
                    Dim extLU As MapWinGIS.Extents = sfLanduse.QuickExtents(j)

                    'see if not possible for shapes to intersect
                    With extSB
                        If Not (.xMax < extLU.xMin OrElse .xMin > extLU.xMax OrElse .yMax < extLU.yMin OrElse .yMin > extLU.yMax) Then
                            Dim shpLanduse As MapWinGIS.Shape = sfLanduse.Shape(j)
                            Try
                                Dim shpInt As MapWinGIS.Shape = MapWinGeoProc.SpatialOperations.Intersection(shpSubbasin, shpLanduse)
                                If Not shpInt Is Nothing Then
                                    If shpInt.numPoints > 0 Then
                                        Dim LandUse As String = sfLanduse.CellValue(fldLanduse, j)
                                        If Not dictSubbasin(ID).ContainsKey(LandUse) Then dictSubbasin(ID).Add(LandUse, 0.0)
                                        dictSubbasin(ID)(LandUse) += MapWinGeoProc.Utils.Area(shpInt)
                                    End If
                                End If
                            Catch ex As Exception
                                NumErrors += 1
                                Debug.Print("Error occurred in TabulateAreas: {0}; land use ID = {1}", ex.Message, sfLanduse.CellValue(fldLanduse, j))
                            End Try
                        End If
                    End With
                    Logger.Progress("Tabulating areas...", j, jmax)
                    RaiseEvent Progress("Tabulating areas...", j, jmax)
                    If Cancel Then Return Nothing
                Next
                Dim subarea As Double = MapWinGeoProc.Utils.Area(shpSubbasin)
                Dim sumarea As Double = 0
                For Each area As Single In dictSubbasin(ID).Values
                    sumarea += area
                Next
                Dim pctdiff As Single = (sumarea - subarea) * 100.0 / subarea
                Logger.Dbg(String.Format("Subbasin: {0}; basin area: {1:0.0}; total LU areas: {2:0.0}; percent error: {3:0.00}", i, subarea, sumarea, pctdiff))
                If NumErrors > 0 Then
                    Logger.Message(String.Format("{0} internal errors occurred while tabulating the areas for subbasin {1}; this may be indicative of a malformed shapefile. The error was ignored but resulted in a discrepancy between the subbasin area and total of all landuse areas of {2:0.00}%.", NumErrors, ID, pctdiff), "Warning", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Warning, Windows.Forms.DialogResult.OK)
                End If
            Next
            Return dictSubbasin
        Catch ex As Exception
            Throw ex
        Finally
            If sfSubbasin IsNot Nothing Then sfSubbasin.Close()
            If sfLanduse IsNot Nothing Then sfLanduse.Close()
            Logger.Progress("", 1, 1)
        End Try
    End Function

    ''' <summary>Given a grid (landuse) and polygon (subbasin) layers, calculate the area of each grid category 
    '''          within each polygon.</summary>
    ''' <remark>This function can be accomplished in MapWindow by 
    '''         looping through each grid cell and counting the 
    '''         number of cells of each grid category within each 
    '''         feature. The MapWinGIS calls to use include the Grid 
    '''         Property Value, the GridHeader Properties dX, dY, XllCenter, 
    '''         and YllCenter, and the Shapefile Function PointInShapefile.</remark>
    ''' <param name="SubbasinLayerName">Name of shapefile (e.g., subbasins)</param>
    ''' <param name="SubbasinFieldName">Name of field containing subbasin ID or Name</param>
    ''' <param name="LanduseLayerName">Name of grid file (e.g., landuse)</param>
    ''' <returns>Output is dictionary (key is polygon ID) of dictionaries (key is grid value, value is area).</returns>
    ''' <remarks>Added by Chris Wilson 5/24/2009</remarks>
    Public Shared Function TabulateAreas(ByVal SubbasinLayerName As String, ByVal SubbasinFieldName As String, _
                                         ByVal LanduseLayerName As String) As Generic.SortedDictionary(Of String, Generic.SortedDictionary(Of String, Single))

        Cancel = False
        Dim sfSubbasin As New MapWinGIS.Shapefile, gLandUse As New MapWinGIS.Grid

        Try
            If Not (IsLayer(LanduseLayerName) AndAlso IsLayer(SubbasinLayerName) AndAlso IsField(LayerIndex(SubbasinLayerName), SubbasinFieldName)) Then Throw New Exception("Layer or field does not exist.")

            If Not sfSubbasin.Open(LayerFileName(SubbasinLayerName)) Then Throw New Exception("Unable to open layer: " & SubbasinLayerName)
            sfSubbasin.BeginPointInShapefile()
            Dim fldSubbasin As Integer = FieldIndex(LayerIndex(SubbasinLayerName), SubbasinFieldName)

            gLandUse.Open(LayerFileName(LanduseLayerName))

            Dim dictSubbasin As New Generic.SortedDictionary(Of String, Generic.SortedDictionary(Of String, Single))

            With gLandUse.Header
                Dim x, y, x0, y0 As Double
                gLandUse.CellToProj(0, 0, x0, y0)
                Dim dx As Double = .dX
                Dim dy As Double = .dY
                Dim gridarea As Double = dx * dy
                y = y0
                For r As Integer = 0 To .NumberRows - 1
                    Dim ar(.NumberCols - 1) As Single
                    gLandUse.GetRow(r, ar(0))
                    x = x0
                    For c As Integer = 0 To .NumberCols - 1
                        Dim LandUse As String = ar(c)
                        Dim shpnum As Integer = sfSubbasin.PointInShapefile(x, y)
                        If shpnum <> -1 Then
                            Dim ID As String = sfSubbasin.CellValue(fldSubbasin, shpnum)
                            If Not dictSubbasin.ContainsKey(ID) Then dictSubbasin.Add(ID, New Generic.SortedDictionary(Of String, Single))
                            With dictSubbasin(ID)
                                If Not .ContainsKey(LandUse) Then .Add(LandUse, 0.0)
                                .Item(LandUse) += gridarea
                            End With
                        End If
                        x += dx
                    Next
                    Logger.Progress("Tabulating areas...", r, .NumberRows - 1)
                    RaiseEvent Progress("Tabulating areas...", r, .NumberRows - 1)
                    If Cancel Then Return Nothing
                    y -= dy
                Next
            End With

            Return dictSubbasin
        Catch ex As Exception
            Throw ex
        Finally
            sfSubbasin.EndPointInShapefile()
            sfSubbasin.Close()
            gLandUse.Close()
            Logger.Progress("", 1, 1)
        End Try
    End Function

    ''' <summary>
    ''' Create unique color scheme for grid layer
    ''' </summary>
    ''' <param name="aLayerIndex"></param>
    ''' <remarks>Added by Chris Wilson 03/26/09 to provide capability for grid layers</remarks>
    Public Shared Sub UniqueValuesRenderer(ByVal aLayerIndex As Integer)
        'build a unique values renderer from a given grid layer 

        Dim lMWlayer As MapWindow.Interfaces.Layer
        lMWlayer = pMapWin.Layers(pMapWin.Layers.GetHandle(aLayerIndex))

        Dim lColorScheme As New MapWinGIS.GridColorScheme
        lMWlayer.DrawFill = True

        Dim g As MapWinGIS.Grid = lMWlayer.GetGridObject

        'Get unique values
        Dim lValuesHt As New Hashtable
        Dim lIndex As Integer
        With g.Header
            Dim NoDataValue As Single = g.Header.NodataValue
            Dim NumRows As Integer = .NumberRows
            Dim NumCols As Integer = .NumberCols
            For r As Integer = 0 To NumRows - 1
                Dim v(NumCols - 1) As Single
                g.GetRow(r, v(0))
                For c As Integer = 0 To NumCols - 1
                    If v(c) <> NoDataValue AndAlso Not lValuesHt.ContainsKey(v(c)) Then lValuesHt.Add(v(c), v(c))
                Next
            Next

            'Create sorted array
            Dim lValuesArray() As Single
            ReDim lValuesArray(lValuesHt.Count - 1)
            lValuesHt.Values().CopyTo(lValuesArray, 0)
            Array.Sort(lValuesArray)

            'Create color for each unique value
            For lIndex = 0 To lValuesArray.Length - 1
                Dim lBreak As New MapWinGIS.GridColorBreak
                lBreak.LowColor = System.Convert.ToUInt32(RGB(CInt(Rnd() * 255), CInt(Rnd() * 255), CInt(Rnd() * 255)))
                lBreak.HighColor = lBreak.LowColor
                lBreak.LowValue = lValuesArray(lIndex) - 0.00001
                lBreak.HighValue = lValuesArray(lIndex) + 0.00001
                lBreak.Caption = lValuesArray(Math.Round(lIndex, 5))
                lColorScheme.InsertBreak(lBreak)
            Next
            lMWlayer.ColoringScheme = lColorScheme
        End With
    End Sub

#End Region

End Class

''' <remarks>Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license</remarks>
''' <summary>The Map Window Object has not been set with code like "GisUtil.MappingObject = ?"</summary>
Public Class MappingObjectNotSetException
    Inherits ApplicationException

    Public Sub New()
        MyBase.New("Mapping Object Not Set")
    End Sub
End Class

Friend Class Downstream
    Friend Shared Sub CheckNeighboringCells(ByVal aFlowDirGrid As MapWinGIS.Grid, ByVal aCol As Integer, ByVal aRow As Integer, ByVal aOutputGrid As MapWinGIS.Grid, Optional ByVal aWeightingFactorGrid As MapWinGIS.Grid = Nothing)
        'used in DownstreamFlowLength, accumulates distances to outlet recursively 

        'figure out the distances x and y
        Dim lX As Double = aOutputGrid.Header.dX
        Dim lY As Double = aOutputGrid.Header.dY
        Dim lDiag As Double = Math.Sqrt((lX ^ 2) + (lY ^ 2))

        If Not aWeightingFactorGrid Is Nothing Then
            lX = lX * aWeightingFactorGrid.Value(aCol, aRow)
            lY = lY * aWeightingFactorGrid.Value(aCol, aRow)
            lDiag = lDiag * aWeightingFactorGrid.Value(aCol, aRow)
        End If

        'this is the assumed flow dir coding from taudem:
        '4 3 2
        '5   1
        '6 7 8
        Dim lnw As Integer = 8
        Dim ln As Integer = 7
        Dim lne As Integer = 6
        Dim lw As Integer = 1
        Dim le As Integer = 5
        Dim lsw As Integer = 2
        Dim ls As Integer = 3
        Dim lse As Integer = 4

        If aFlowDirGrid.Maximum > 8 Then
            'assume arcview coding:
            '32 64 128
            '16      1
            ' 8  4   2
            lnw = 2
            ln = 4
            lne = 8
            lw = 1
            le = 16
            lsw = 128
            ls = 64
            lse = 32
        End If

        'check nw cell
        If aRow - 1 > 0 And aCol - 1 > 0 Then
            If aFlowDirGrid.Value(aCol - 1, aRow - 1) = lnw Then
                aOutputGrid.Value(aCol - 1, aRow - 1) = lDiag + aOutputGrid.Value(aCol, aRow)
                'If aOutputGrid.Value(aCol, aRow) < 0 Then
                '    'Logger.Dbg("negative value for output grid")
                'End If
                CheckNeighboringCells(aFlowDirGrid, aCol - 1, aRow - 1, aOutputGrid, aWeightingFactorGrid)
            End If
        End If

        'check n cell
        If aRow - 1 > 0 Then
            If aFlowDirGrid.Value(aCol, aRow - 1) = ln Then
                aOutputGrid.Value(aCol, aRow - 1) = lY + aOutputGrid.Value(aCol, aRow)
                'If aOutputGrid.Value(aCol, aRow) < 0 Then
                '    'Logger.Dbg("negative value for output grid")
                'End If
                CheckNeighboringCells(aFlowDirGrid, aCol, aRow - 1, aOutputGrid, aWeightingFactorGrid)
            End If
        End If

        'check ne cell
        If aRow - 1 > 0 And aCol + 1 < aFlowDirGrid.Header.NumberCols Then
            If aFlowDirGrid.Value(aCol + 1, aRow - 1) = lne Then
                aOutputGrid.Value(aCol + 1, aRow - 1) = lDiag + aOutputGrid.Value(aCol, aRow)
                'If aOutputGrid.Value(aCol, aRow) < 0 Then
                '    'Logger.Dbg("negative value for output grid")
                'End If
                CheckNeighboringCells(aFlowDirGrid, aCol + 1, aRow - 1, aOutputGrid, aWeightingFactorGrid)
            End If
        End If

        'check w cell
        If aCol - 1 > 0 Then
            If aFlowDirGrid.Value(aCol - 1, aRow) = lw Then
                aOutputGrid.Value(aCol - 1, aRow) = lX + aOutputGrid.Value(aCol, aRow)
                'If aOutputGrid.Value(aCol, aRow) < 0 Then
                '    'Logger.Dbg("negative value for output grid")
                'End If
                CheckNeighboringCells(aFlowDirGrid, aCol - 1, aRow, aOutputGrid, aWeightingFactorGrid)
            End If
        End If

        'check e cell
        If aCol + 1 < aFlowDirGrid.Header.NumberCols Then
            If aFlowDirGrid.Value(aCol + 1, aRow) = le Then
                aOutputGrid.Value(aCol + 1, aRow) = lX + aOutputGrid.Value(aCol, aRow)
                'If aOutputGrid.Value(aCol, aRow) < 0 Then
                '    'Logger.Dbg("negative value for output grid")
                'End If
                CheckNeighboringCells(aFlowDirGrid, aCol + 1, aRow, aOutputGrid, aWeightingFactorGrid)
            End If
        End If

        'check sw cell
        If aRow + 1 < aFlowDirGrid.Header.NumberRows And aCol - 1 > 0 Then
            If aFlowDirGrid.Value(aCol - 1, aRow + 1) = lsw Then
                aOutputGrid.Value(aCol - 1, aRow + 1) = lDiag + aOutputGrid.Value(aCol, aRow)
                'If aOutputGrid.Value(aCol, aRow) < 0 Then
                '    'Logger.Dbg("negative value for output grid")
                'End If
                CheckNeighboringCells(aFlowDirGrid, aCol - 1, aRow + 1, aOutputGrid, aWeightingFactorGrid)
            End If
        End If

        'check s cell
        If aRow + 1 < aFlowDirGrid.Header.NumberRows Then
            If aFlowDirGrid.Value(aCol, aRow + 1) = ls Then
                aOutputGrid.Value(aCol, aRow + 1) = lY + aOutputGrid.Value(aCol, aRow)
                'If aOutputGrid.Value(aCol, aRow) < 0 Then
                '    'Logger.Dbg("negative value for output grid")
                'End If
                CheckNeighboringCells(aFlowDirGrid, aCol, aRow + 1, aOutputGrid, aWeightingFactorGrid)
            End If
        End If

        'check se cell
        If aRow + 1 < aFlowDirGrid.Header.NumberRows And aCol + 1 < aFlowDirGrid.Header.NumberCols Then
            If aFlowDirGrid.Value(aCol + 1, aRow + 1) = lse Then
                aOutputGrid.Value(aCol + 1, aRow + 1) = lDiag + aOutputGrid.Value(aCol, aRow)
                'If aOutputGrid.Value(aCol, aRow) < 0 Then
                '    'Logger.Dbg("negative value for output grid")
                'End If
                CheckNeighboringCells(aFlowDirGrid, aCol + 1, aRow + 1, aOutputGrid, aWeightingFactorGrid)
            End If
        End If

    End Sub
End Class

