Imports atcUtility

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
    MapLayers = New ArrayList
    Dim lLastLayerIndex As Integer = GetMappingObject.Layers.NumLayers - 1
    For lLayerIndex As Integer = 0 To lLastLayerIndex
      MapLayers.Add(GetMappingObject.Layers(lLayerIndex))
    Next
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
  '''     <para>Filename of project to load</para>
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
  '''     <para>Index of Layer containing ShapeFile</para>
  ''' </param>
  ''' <exception cref="Exception.html#LayerNotShapeFile" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
  ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
  ''' <exception cref="Exception.html#MappingObjectNotSet" caption="MappingObjectNotSet">Mapping Object Not Set</exception>
  Private Shared ReadOnly Property ShapeFileFromIndex(Optional ByVal aLayerIndex As Integer = UseCurrent) As MapWinGIS.Shapefile
    Get
      If aLayerIndex = UseCurrent Then aLayerIndex = CurrentLayer

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
      If aLayerIndex = UseCurrent Then aLayerIndex = CurrentLayer

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
      If aLayerIndex = UseCurrent Then aLayerIndex = CurrentLayer

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
    End If

    Dim lSf2 As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aLayerIndex2)
    Dim lSf2Shape As MapWinGIS.Shape
    If FeatureIndexValid(aFeatureIndex2, lSf2) Then
      lSf2Shape = lSf2.Shape(aFeatureIndex2)
    End If

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

    Dim lSf1 As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aLayerIndex)
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
  Public Shared Function RemoveField(ByVal aLayerIndex As Integer, ByVal aFieldIndex As Integer)
    Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)

    If FieldIndexValid(aFieldIndex, lSf) Then
      lSf.StartEditingTable()
      'TODO: error handling
      Dim lBsuc As Boolean = lSf.EditDeleteField(aFieldIndex)
      lSf.StopEditingTable()
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
    ElseIf FeatureIndexValid(aFeatureIndex, lSf) Then
      Return lSf.CellValue(aFieldIndex, aFeatureIndex)
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
      Dim lUtils As New MapWinGIS.Utils
      Try
        lArea = lUtils.Area(lSf.Shape(aFeatureIndex))
        If lArea < 0.000001 Then 'TODO: try to calculate?
          lArea = Double.NaN
        End If
      Catch ex As Exception
        LogDbg("GisUtil:FeatureArea:Exception:" & ex.Message)
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
          Dim lShapeFileType = lSf.ShapefileType
          If lShapeFileType <> MapWinGIS.ShpfileType.SHP_POLYLINE AndAlso _
             lShapeFileType <> MapWinGIS.ShpfileType.SHP_POLYLINEM AndAlso _
             lShapeFileType <> MapWinGIS.ShpfileType.SHP_POLYLINEZ Then
            LogDbg("GisUtil:FeatureLength:LengthUndefinedForShapeType:" & lSf.ShapefileType)
            lLength = Double.NaN
          End If
        End If
      End If
    Catch ex As Exception
      LogDbg("GisUtil:FeatureLength:Exception:" & ex.Message)
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
    Dim lsf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)

    If FeatureIndexValid(aFeatureIndex, lsf) Then
      lsf.StartEditingShapes(True)
      Dim bsuc As Boolean = lsf.EditDeleteShape(aFeatureIndex)
      lsf.StopEditingShapes(True, True)
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
    Dim lsf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
    If FeatureIndexValid(aFeatureIndex, lsf) Then
      If FieldIndexValid(aFieldIndex, lsf) Then
        Dim bsuc As Boolean
        'TODO: error checks
        bsuc = lsf.StartEditingTable()
        bsuc = lsf.EditCellValue(aFieldIndex, aFeatureIndex, aValue)
        bsuc = lsf.StopEditingTable()
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

  Public Shared Function IndexOfNthSelectedFeatureInLayer(ByVal nth As Integer, ByVal aLayerIndex As Integer) As Integer
    Dim lsi As MapWindow.Interfaces.SelectInfo = GetMappingObject.View.SelectedShapes
    Dim lLayer As MapWindow.Interfaces.Layer = LayerFromIndex(aLayerIndex)
    If lsi.LayerHandle = aLayerIndex Then
      IndexOfNthSelectedFeatureInLayer = lsi.Item(nth).ShapeIndex
    End If
  End Function

  Public shared Sub SaveSelectedFeatures(ByVal aInputLayerName As String, ByVal aOutputLayerName As String)
    'save the selected features of a layer as a new shapefile
    Dim i As Integer
    Dim j As Integer
    Dim k As Integer
    Dim InputLayerIndex As Integer
    Dim bsuc As Boolean

    InputLayerIndex = LayerIndex(aInputLayerName)
    Dim lsf As MapWinGIS.Shapefile = ShapeFileFromIndex(InputLayerIndex)
    
    Dim osf As New MapWinGIS.Shapefile
    'create new shapefile
    osf.CreateNew(aOutputLayerName, lsf.ShapefileType)
    For i = 1 To lsf.NumFields
      Dim of As New MapWinGIS.Field
      of.Name = lsf.Field(i - 1).Name
      of.Type = lsf.Field(i - 1).Type
      of.Width = lsf.Field(i - 1).Width
      of.Precision = lsf.Field(i - 1).Precision
      bsuc = osf.EditInsertField(of, osf.NumFields)
      of = Nothing
    Next i
    osf.StartEditingShapes(True)

    Dim lsi As MapWindow.Interfaces.SelectInfo
    lsi = pMapWin.View.SelectedShapes
    If lsi.LayerHandle = InputLayerIndex Then
      For i = 1 To lsf.NumShapes
        'loop through each shape of the layer
        For j = 1 To lsi.NumSelected
          'loop through selected shapes
          If i - 1 = lsi.Item(j - 1).ShapeIndex Then
            osf.EditInsertShape(lsf.Shape(i - 1), osf.NumShapes)
            For k = 1 To osf.NumFields
              bsuc = osf.EditCellValue(k - 1, osf.NumShapes - 1, lsf.CellValue(k - 1, i - 1))
            Next k
          End If
        Next j
      Next i
    End If

    bsuc = osf.SaveAs(aOutputLayerName)
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

  Public Shared Function AddLayer(ByVal aFileName As String, ByVal aLayerName As String) As Boolean
    'given a shape file name, add it to the map.
    'return true if the layer is already there or successfully added.
    If LayerIndex(aLayerName) > -1 Then 'already on map 
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

  Public Shared Sub TabulateAreas(ByVal aGridLayerIndex As Integer, _
                                  ByVal aPolygonLayerIndex As Integer, _
                                  ByVal aAreaGridPoly(,) As Double)
    'Given a grid and a polygon layer, calculate the area of each grid category 
    'within each polygon.  Output array contains area of each grid category
    'and polygon combination.

    'This function can be accomplished in MapWindow by 
    'looping through each grid cell and counting the 
    'number of cells of each grid category within each 
    'feature. The MapWinGIS calls to use include the Grid 
    'Property Value, the GridHeader Properties dX, dY, XllCenter, 
    'and YllCenter, and the Shapefile Function PointInShapefile. 
    Dim ic As Integer
    Dim ir As Integer
    Dim xpos As Double
    Dim ypos As Double
    Dim subid As Integer
    Dim luid As Integer
    Dim lcellarea As Double
    'Dim totalcellcount As Integer
    'Dim cellcount As Integer
    'Dim lastdisplayed As Integer
    Dim startingcolumn As Integer
    Dim endingcolumn As Integer
    Dim startingrow As Integer
    Dim endingrow As Integer

    'set input grid
    Dim InputGrid As MapWinGIS.Grid = GridFromIndex(aGridLayerIndex)
    'set input polygon layer
    Dim lPolygonSf As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aPolygonLayerIndex)
    'figure out what part of the grid overlays these polygons
    InputGrid.ProjToCell(lPolygonSf.Extents.xMin, lPolygonSf.Extents.yMin, startingcolumn, endingrow)
    InputGrid.ProjToCell(lPolygonSf.Extents.xMax, lPolygonSf.Extents.yMax, endingcolumn, startingrow)

    lcellarea = InputGrid.Header.dX * InputGrid.Header.dY
    'totalcellcount = (endingcolumn - startingcolumn) * (endingrow - startingrow)
    'cellcount = 0
    'lastdisplayed = 0

    lPolygonSf.BeginPointInShapefile()
    For ic = startingcolumn To endingcolumn
      For ir = startingrow To endingrow
        InputGrid.CellToProj(ic, ir, xpos, ypos)
        subid = lPolygonSf.PointInShapefile(xpos, ypos)
        If subid > -1 Then
          'this is in a subbasin
          If InputGrid.Value(ic, ir).GetType.Name = "SByte" Then
            luid = Convert.ToInt32(InputGrid.Value(ic, ir))
          Else
            luid = InputGrid.Value(ic, ir)
          End If
          aAreaGridPoly(luid, subid) = aAreaGridPoly(luid, subid) + lcellarea
        End If
        'cellcount = cellcount + 1
      Next ir
      'If Int(cellcount / totalcellcount * 100) > lastdisplayed Then
      '  lblStatus.Text = "Overlaying Land Use and Subbasins (" & Int(cellcount / totalcellcount * 100) & "%)"
      '  Me.Refresh()
      '  lastdisplayed = Int(cellcount / totalcellcount * 100)
      'End If
    Next ic
    lPolygonSf.EndPointInShapefile()
  End Sub

  Public Shared Sub GridMinMaxInPolygon(ByVal aGridLayerIndex As Integer, ByVal aPolygonLayerIndex As Integer, _
                                        ByVal aPolygonFeatureIndex As Integer, _
                                        ByVal aMin As Double, ByVal aMax As Double)
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

    'set input grid
    Dim lInputGrid As MapWinGIS.Grid = GridFromIndex(aGridLayerIndex)

    'set input polygon layer
    Dim lPolygonSf As MapWinGIS.Shapefile = PolygonShapeFileFromIndex(aPolygonLayerIndex)
    Dim lShape As New MapWinGIS.Shape
    If FeatureIndexValid(aPolygonFeatureIndex, lPolygonsf) Then
      lShape = lPolygonSf.Shape(aPolygonFeatureIndex)

      'figure out what part of the grid overlays this polygon
      lInputGrid.ProjToCell(lShape.Extents.xMin, lShape.Extents.yMin, lStartCol, lEndRow)
      lInputGrid.ProjToCell(lShape.Extents.xMax, lShape.Extents.yMax, lEndCol, lStartRow)

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
        Next lRow
      Next lCol
      lPolygonSf.EndPointInShapefile()
    End If
  End Sub

  Public Shared Function GridValueAtPoint(ByVal GridLayerIndex As Integer, ByVal x As Double, ByVal y As Double) As Integer
    Dim column As Integer
    Dim row As Integer
    Dim endingrow As Integer
    Dim gridLayer As MapWindow.Interfaces.Layer

    'set input grid
    gridLayer = GetMappingObject.Layers(GridLayerIndex)
    Dim InputGrid As New MapWinGIS.Grid
    InputGrid = gridLayer.GetGridObject

    InputGrid.ProjToCell(x, y, column, row)
    GridValueAtPoint = InputGrid.Value(column, row)
  End Function

  Public Shared Sub Overlay(ByVal Layer1Name As String, ByVal Layer1FieldName As String, _
                             ByVal Layer2Name As String, ByVal Layer2FieldName As String, _
                             ByVal OutputLayerName As String, ByVal CreateNew As Boolean)
    'overlay layer1 and layer2 (eg landuse and subbasins), creating a polygon layer 
    'containing features from both layers

    Dim i As Integer
    Dim k As Integer
    Dim shapeindex As Integer
    Dim Layer1Index As Integer
    Dim Layer1FieldIndex As Integer
    Dim Layer2Index As Integer
    Dim Layer2FieldIndex As Integer
    Dim totalpolygoncount As Integer
    Dim polygoncount As Integer
    Dim lastdisplayed As Integer
    Dim SSFext As MapWinGIS.Extents
    Dim LUext As MapWinGIS.Extents
    Dim area As Double
    Dim Feature1Id As String
    Dim Feature2Id As String
    Dim bsuc As Boolean

    'set layer 1 (landuse)
    Layer1Index = LayerIndex(Layer1Name)
    Layer1FieldIndex = FieldIndex(Layer1Index, Layer1FieldName)
    Dim lLayer As MapWindow.Interfaces.Layer
    lLayer = GetMappingObject.Layers(Layer1Index)
    Dim lusf As New MapWinGIS.Shapefile
    lusf = lLayer.GetObject

    'set layer 2 (subbasins)
    Layer2Index = LayerIndex(Layer2Name)
    Layer2FieldIndex = FieldIndex(Layer2Index, Layer2FieldName)
    Dim sLayer As MapWindow.Interfaces.Layer
    sLayer = GetMappingObject.Layers(Layer2Index)
    Dim ssf As New MapWinGIS.Shapefile
    ssf = sLayer.GetObject
    SSFext = ssf.Extents

    'if any of layer 2 is selected, use only those
    Dim cSelectedSubbasins As New Collection
    For i = 1 To NumSelectedFeatures(Layer2Index)
      cSelectedSubbasins.Add(IndexOfNthSelectedFeatureInLayer(i - 1, Layer2Index))
    Next
    If cSelectedSubbasins.Count = 0 Then
      'no subbasins selected, act as if all are selected
      For i = 1 To NumFeatures(Layer2Index)
        cSelectedSubbasins.Add(i - 1)
      Next
    End If

    Dim osf As New MapWinGIS.Shapefile
    If CreateNew Then
      'create new overlay shapefile
      osf.CreateNew("overlay", MapWinGIS.ShpfileType.SHP_POLYGON)
      Dim of As New MapWinGIS.Field
      of.Name = Layer1FieldName
      of.Type = MapWinGIS.FieldType.INTEGER_FIELD
      of.Width = 10
      bsuc = osf.EditInsertField(of, 0)
      Dim of2 As New MapWinGIS.Field
      of2.Name = Layer2FieldName
      of2.Type = MapWinGIS.FieldType.INTEGER_FIELD
      of2.Width = 10
      bsuc = osf.EditInsertField(of2, 1)
      Dim of3 As New MapWinGIS.Field
      of3.Name = "Area"
      of3.Type = MapWinGIS.FieldType.DOUBLE_FIELD
      of3.Width = 10
      of3.Precision = 3
      bsuc = osf.EditInsertField(of3, 2)
    Else
      'open existing
      bsuc = osf.Open(OutputLayerName)
    End If
    osf.StartEditingShapes(True)

    Dim utilClip As New MapWinGIS.Utils
    Dim utilArea As New MapWinGIS.Utils
    Dim newshape As MapWinGIS.Shape
    Dim lusfshape As MapWinGIS.Shape
    Dim ssfshape As MapWinGIS.Shape
    Dim ssfshapeext As MapWinGIS.Extents

    'set up collections of subbasin shapes and extents to save computation time later
    Dim cSsfShape As New Collection
    Dim cSsfShapeExtXmax As New Collection
    Dim cSsfShapeExtXmin As New Collection
    Dim cSsfShapeExtYmax As New Collection
    Dim cSsfShapeExtYmin As New Collection
    For k = 1 To cSelectedSubbasins.Count
      'loop thru each selected subbasin (or all if none selected)
      shapeindex = cSelectedSubbasins(k)
      ssfshape = ssf.Shape(shapeindex)
      ssfshapeext = ssfshape.Extents
      cSsfShape.Add(ssfshape)
      cSsfShapeExtXmax.Add(ssfshapeext.xMax)
      cSsfShapeExtXmin.Add(ssfshapeext.xMin)
      cSsfShapeExtYmax.Add(ssfshapeext.yMax)
      cSsfShapeExtYmin.Add(ssfshapeext.yMin)
    Next k

    '********** do overlay ***********
    GetMappingObject.StatusBar.ShowProgressBar = True
    totalpolygoncount = lusf.NumShapes * cSelectedSubbasins.Count
    polygoncount = 0
    lastdisplayed = 0
    GetMappingObject.StatusBar.ProgressBarValue = 0
    For i = 1 To lusf.NumShapes
      'loop through each shape of the land use layer
      lusfshape = lusf.Shape(i - 1)
      LUext = lusfshape.Extents
      'see if the current landuse polygon falls in the extents of the subbasin shapefile
      If Not (LUext.xMin > SSFext.xMax Or LUext.xMax < SSFext.xMin Or LUext.yMin > SSFext.yMax Or LUext.yMax < SSFext.yMin) Then
        For k = 1 To cSelectedSubbasins.Count
          'loop thru each selected subbasin (or all if none selected)
          shapeindex = cSelectedSubbasins(k)
          polygoncount = polygoncount + 1
          If Not (LUext.xMin > cSsfShapeExtXmax(k) Or LUext.xMax < cSsfShapeExtXmin(k) Or LUext.yMin > cSsfShapeExtYmax(k) Or LUext.yMax < cSsfShapeExtYmin(k)) Then
            'look for intersection from overlay of these shapes
            newshape = utilClip.ClipPolygon(MapWinGIS.PolygonOperation.INTERSECTION_OPERATION, lusfshape, ssfshape)
            If newshape.numPoints > 0 Then
              'Insert the shape into the shapefile 
              osf.EditInsertShape(newshape, osf.NumShapes)
              area = Math.Abs(utilArea.Area(newshape))
              'keep track of which subbasin and land use class
              Feature1Id = lusf.CellValue(Layer1FieldIndex, i - 1)
              Feature2Id = ssf.CellValue(Layer2FieldIndex, shapeindex)
              bsuc = osf.EditCellValue(0, osf.NumShapes - 1, Feature1Id)
              bsuc = osf.EditCellValue(1, osf.NumShapes - 1, Feature2Id)
              bsuc = osf.EditCellValue(2, osf.NumShapes - 1, area)
            End If
            newshape = Nothing
          End If
        Next k
      Else
        polygoncount = polygoncount + cSelectedSubbasins.Count
      End If
      LUext = Nothing
      If Int(polygoncount / totalpolygoncount * 100) > lastdisplayed Then
        'lblStatus.Text = "Overlaying Land Use and Subbasins (" & Int(polygoncount / totalpolygoncount * 100) & "%)"
        'Me.Refresh()
        lastdisplayed = Int(polygoncount / totalpolygoncount * 100)
        GetMappingObject.StatusBar.ProgressBarValue = Int(polygoncount / totalpolygoncount * 100)
      End If
      lusfshape = Nothing
    Next i
    GetMappingObject.StatusBar.ShowProgressBar = False

    If CreateNew Then
      'delete old version of this file if it exists
      If FileExists(OutputLayerName) Then
        System.IO.File.Delete(OutputLayerName)
      End If
      If FileExists(FilenameNoExt(OutputLayerName) & ".shx") Then
        System.IO.File.Delete(FilenameNoExt(OutputLayerName) & ".shx")
      End If
      If FileExists(FilenameNoExt(OutputLayerName) & ".dbf") Then
        System.IO.File.Delete(FilenameNoExt(OutputLayerName) & ".dbf")
      End If
    End If
    bsuc = osf.SaveAs(OutputLayerName)
    osf.StopEditingShapes()
    osf.Close()

  End Sub

  Public Shared Function ClipShapesWithPolygon(ByVal inputlayerindex As Integer, ByVal clipperlayerindex As Integer) As String
    'returns output shape file name as string 
    Dim mx As MapWinX.SpatialOperations
    Dim i As Integer
    Dim j As Integer
    Dim k As Integer
    Dim bsuc As Boolean
    Dim newname As String

    ClipShapesWithPolygon = ""
    'set input layer (reaches for instance)
    Dim isf As MapWinGIS.Shapefile = ShapeFileFromIndex(inputlayerindex)

    'set clipper layer (subbasins for instance)
    Dim ssf As MapWinGIS.Shapefile = ShapeFileFromIndex(clipperlayerindex)

    Dim sshape As MapWinGIS.Shape
    Dim clipperpath As String = PathNameOnly(LayerFileName(clipperlayerindex))

    'create results shapefile
    i = 1
    newname = clipperpath & "\stream" & i & ".shp"
    Do While FileExists(newname)
      i = i + 1
      newname = clipperpath & "\stream" & i & ".shp"
    Loop
    Dim orsf As New MapWinGIS.Shapefile
    Dim rsf As New MapWinGIS.Shapefile
    orsf.CreateNew(newname, MapWinGIS.ShpfileType.SHP_POLYLINE)
    For i = 1 To isf.NumFields
      bsuc = orsf.EditInsertField(isf.Field(i - 1), i - 1)
    Next i

    GetMappingObject.StatusBar.ShowProgressBar = True
    For i = 1 To ssf.NumShapes
      GetMappingObject.StatusBar.ProgressBarValue = Int(i / ssf.NumShapes * 100)
      sshape = ssf.Shape(i - 1)
      bsuc = mx.ClipShapesWithPolygon(isf, sshape, rsf)
      For j = 1 To rsf.NumShapes
        bsuc = orsf.EditInsertShape(rsf.Shape(j - 1), j - 1)
      Next j
    Next i
    GetMappingObject.StatusBar.ShowProgressBar = False

    'populate attributes of output shapes
    For i = 1 To orsf.NumShapes
      For j = 1 To isf.NumShapes
        If IsThisShapeTheSameAsOrPartOfAnotherShape(orsf.Shape(i - 1), isf.Shape(j - 1)) Then
          'found matching shape, copy attributes
          For k = 1 To isf.NumFields
            bsuc = orsf.EditCellValue(k - 1, i - 1, isf.CellValue(k - 1, j - 1))
          Next k
        End If
      Next j
    Next i

    bsuc = orsf.SaveAs(newname)
    ClipShapesWithPolygon = newname

  End Function

  Private Shared Function IsThisShapeTheSameAsOrPartOfAnotherShape(ByVal sf1shape As MapWinGIS.Shape, ByVal sf2shape As MapWinGIS.Shape) As Boolean
    'given one shape and another which may be a piece of the first shape,
    'determine if the second shape is equivalent or part of the first shape
    '(after a reach segment has been clipped to a polygon boundary, we still need 
    'to identify which original reach it was a part of)
    Dim i As Integer
    Dim j As Integer
    Dim foundpoint As Boolean

    IsThisShapeTheSameAsOrPartOfAnotherShape = True

    For j = 2 To sf1shape.numPoints - 1
      foundpoint = False
      For i = 1 To sf2shape.numPoints
        If sf1shape.Point(j - 1).x = sf2shape.Point(i - 1).x And sf1shape.Point(j - 1).y = sf2shape.Point(i - 1).y Then
          foundpoint = True
          Exit For
        End If
      Next i
      If foundpoint = False Then
        IsThisShapeTheSameAsOrPartOfAnotherShape = False
        Exit Function
      End If
    Next j

  End Function

  Public Shared Sub MergeFeaturesBasedOnAttribute(ByVal aLayerIndex, ByVal FieldIndex)
    Dim i As Integer
    Dim j As Integer
    Dim k As Integer
    Dim bsuc As Boolean
    Dim thisval As Integer
    Dim targetval As Integer
    Dim shape1 As MapWinGIS.Shape
    Dim shape2 As MapWinGIS.Shape
    Dim endx As Double
    Dim endy As Double
    Dim found As Boolean

    Dim isf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)

    isf.StartEditingShapes(True)
    'merge together based on common endpoints
    i = 0
    Do While i < isf.NumShapes
      found = False
      shape1 = isf.Shape(i)
      targetval = FieldValue(aLayerIndex, i, FieldIndex)
      endx = shape1.Point(shape1.numPoints - 1).x
      endy = shape1.Point(shape1.numPoints - 1).y
      j = 0
      Do While j < isf.NumShapes
        If i <> j Then
          shape2 = isf.Shape(j)
          thisval = FieldValue(aLayerIndex, j, FieldIndex)
          If thisval = targetval Then
            'see if these have common start/end 
            If endx = shape2.Point(0).x And endy = shape2.Point(0).y Then
              'end of shape 1 is start of shape2
              For k = 1 To shape2.numPoints
                bsuc = shape1.InsertPoint(shape2.Point(k), shape1.numPoints)
              Next k
              'remove shape2
              isf.EditDeleteShape(j)
              found = True
              Exit Do
            End If
          End If
        End If
        j = j + 1
      Loop
      If Not found Then
        i = i + 1
      End If
    Loop

    'merge together based on endpoint proximity
    Dim startx1 As Double
    Dim starty1 As Double
    Dim endx1 As Double
    Dim endy1 As Double
    Dim startx2 As Double
    Dim starty2 As Double
    Dim endx2 As Double
    Dim endy2 As Double
    i = 0
    Do While i < isf.NumShapes
      found = False
      shape1 = isf.Shape(i)
      targetval = FieldValue(aLayerIndex, i, FieldIndex)
      startx1 = shape1.Point(0).x
      starty1 = shape1.Point(0).y
      endx1 = shape1.Point(shape1.numPoints - 1).x
      endy1 = shape1.Point(shape1.numPoints - 1).y
      j = 0
      Do While j < isf.NumShapes
        If i <> j Then
          shape2 = isf.Shape(j)
          thisval = FieldValue(aLayerIndex, j, FieldIndex)
          If thisval = targetval Then
            startx2 = shape2.Point(0).x
            starty2 = shape2.Point(0).y
            endx2 = shape2.Point(shape2.numPoints - 1).x
            endy2 = shape2.Point(shape2.numPoints - 1).y
            If (startx1 - endx2) ^ 2 + (starty1 - endy2) ^ 2 < (startx2 - endx1) ^ 2 + (starty2 - endy1) ^ 2 Then
              'add shape 1 to the end of shape 2
              For k = 1 To shape1.numPoints
                bsuc = shape2.InsertPoint(shape1.Point(k), shape2.numPoints)
              Next k
              'remove shape1
              isf.EditDeleteShape(i)
              found = True
              Exit Do
            Else
              'add shape 2 to the end of shape 1
              For k = 1 To shape2.numPoints
                bsuc = shape1.InsertPoint(shape2.Point(k), shape1.numPoints)
              Next k
              'remove shape2
              isf.EditDeleteShape(j)
              found = True
              Exit Do
            End If
          End If
        End If
        j = j + 1
      Loop
      If Not found Then
        i = i + 1
      End If
    Loop

    isf.StopEditingShapes(True)
  End Sub

End Class
