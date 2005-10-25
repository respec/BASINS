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
  Public Shared Property MappingObject() As MapWindow.Interfaces.IMapWin
    Get
      If pMapWin Is Nothing Then
        Throw New Exception("GisUtil:Mapping Object Not Set")
      Else
        Return pMapWin
      End If
    End Get
    Set(ByVal aNewValue As MapWindow.Interfaces.IMapWin)
      pMapWin = aNewValue
    End Set
  End Property

  ''' <summary>Load MapWindow project</summary>
  ''' <param name="aProjectName">
  '''     <para>Filename of project to load</para>
  ''' </param>
  ''' <remarks>Current directory not changed</remarks>
  ''' <exception cref="Exception.html#LoadFailure" caption="LoadFailure">Failure to Load Project</exception>
  ''' <exception cref="Exception.html#FileNotFound" caption="FileNotFound">File Not Found</exception>
  Public Shared Sub LoadProject(ByVal aProjectName As String)
    If FileExists(aProjectName) Then
      Dim lBaseDir As String = CurDir()  'dont want to change curdir, save original
      Dim lRet As Boolean = MappingObject.Project.Load(aProjectName)
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
  Public Shared Property CurrentLayer() As Integer
    Get
      Return MappingObject.Layers.CurrentLayer
    End Get
    Set(ByVal aLayerIndex As Integer)
      If Not LayerFromIndex(aLayerIndex) Is Nothing Then
        MappingObject.Layers.CurrentLayer = aLayerIndex
      End If
    End Set
  End Property

  ''' <summary>Obtain pointer to a shape file from a LayerIndex</summary>
  ''' <param name="aLayerIndex">
  '''     <para>Index of Layer containing ShapeFile</para>
  ''' </param>
  ''' <exception cref="Exception.html#LayerNotShapeFile" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
  ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
  Private Shared ReadOnly Property ShapeFileFromIndex(Optional ByVal aLayerIndex As Integer = UseCurrent) As MapWinGIS.Shapefile
    Get
      If aLayerIndex = UseCurrent Then aLayerIndex = CurrentLayer

      Dim lLayer As MapWindow.Interfaces.Layer = LayerFromIndex(aLayerIndex)
      If lLayer.LayerType = MapWindow.Interfaces.eLayerType.LineShapefile OrElse _
        lLayer.LayerType = MapWindow.Interfaces.eLayerType.PointShapefile OrElse _
        lLayer.LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
        Return lLayer.GetObject
      Else
        Throw New Exception("GisUtil:ShapeFileFromIndex:Error:LayerIndex:" & aLayerIndex & ":Type:" & MappingObject.Layers(aLayerIndex).LayerType & ":IsNotShapeFile")
      End If
    End Get
  End Property

  ''' <summary>Obtain pointer to a grid from a LayerIndex</summary>
  ''' <param name="aLayerIndex">
  '''     <para>Index of desired layer containing grid (defaults to Current Layer)</para>
  ''' </param>
  ''' <exception cref="Exception.html#LayerNotGrid" caption="LayerNotGrid">Layer specified by aLayerIndex is not a Grid</exception>
  ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
  Private Shared ReadOnly Property GridFromIndex(Optional ByVal aLayerIndex As Integer = UseCurrent) As MapWinGIS.Grid
    Get
      If aLayerIndex = UseCurrent Then aLayerIndex = CurrentLayer

      Dim lLayer As MapWindow.Interfaces.Layer = LayerFromIndex(aLayerIndex)
      If lLayer.LayerType = MapWindow.Interfaces.eLayerType.Grid Then
        Return lLayer.GetGridObject
      Else
        Throw New Exception("GisUtil:GridFromIndex:Error:LayerIndex:" & aLayerIndex & ":Type:" & MappingObject.Layers(aLayerIndex).LayerType & ":IsNotGrid")
      End If
    End Get
  End Property

  ''' <summary>Obtain pointer to a layer from a LayerIndex</summary>
  ''' <param name="aLayerIndex">
  '''     <para>Index of desired layer (defaults to Current Layer)</para>
  ''' </param>
  ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
  Private Shared ReadOnly Property LayerFromIndex(Optional ByVal aLayerIndex As Integer = UseCurrent) As MapWindow.Interfaces.Layer
    Get
      If aLayerIndex = UseCurrent Then aLayerIndex = CurrentLayer

      If aLayerIndex >= 0 And aLayerIndex < MappingObject.Layers.NumLayers Then
        Return (MappingObject.Layers(aLayerIndex))
      Else
        Throw New Exception("GisUtil:LayerFromIndex:Error:LayerIndex:" & aLayerIndex & ":OutOfRange:0:" & MappingObject.Layers.NumLayers - 1)
      End If

    End Get
  End Property

  Public Shared Function OverlappingPolygons( _
    ByVal aLayerIndex1 As Integer, ByVal aFeatureIndex1 As Integer, _
    ByVal aLayerIndex2 As Integer, ByVal aFeatureIndex2 As Integer) As Boolean

    OverlappingPolygons = False

    Dim lSf1 As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex1)
    Dim lSf1Shape As MapWinGIS.Shape = lSf1.Shape(aFeatureIndex1)

    Dim lSf2 As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex2)
    Dim lSf2Shape As MapWinGIS.Shape = lSf2.Shape(aFeatureIndex2)

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

  Public Shared Sub AssignContainingPolygons( _
    ByVal aLayerIndex As Integer, _
    ByVal aLayerIndexContaining As Integer, _
    ByRef aIndex() As Integer)
    'given a polygon layer (like dem shape) and a containing layer (like subbasins),
    'return which polygon in the containing layer each polygon lies within

    Dim lSf1 As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
    Dim lSf2 As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndexContaining)

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
  Public Shared ReadOnly Property NumFields(Optional ByVal aLayerIndex As Integer = UseCurrent) As Integer
    Get
      Return ShapeFileFromIndex(aLayerIndex).NumFields
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
  ''' <exception caption="Exception.html#FieldIndexOutOfRange">Field specified by aFieldIndex does not exist</exception>
  Public Shared ReadOnly Property FieldName(ByVal aFieldIndex As Integer, ByVal aLayerIndex As Integer) As String
    Get
      Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
      If aFieldIndex < lSf.NumFields AndAlso aFieldIndex >= 0 Then
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

  ''' <summary>Add a field in a shape file from a layer index and a field name</summary>
  ''' <param name="aLayerIndex">
  '''     <para>Index of layer containing shape file</para>
  ''' </param>
  ''' <param name="aFieldIndex">
  '''     <para>Index of field to remove</para>
  ''' </param>
  ''' <exception cref="Exception.html#LayerNotShapeFile" caption="LayerNotShapeFile">Layer specified by aLayerIndex is not a ShapeFile</exception>
  ''' <exception cref="Exception.html#LayerIndexOutOfRange" caption="LayerIndexOutOfRange">Layer specified by aLayerIndex does not exist</exception>
  Public Shared Function RemoveField(ByVal aLayerIndex As Integer, ByVal aFieldIndex As Integer)
    Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)

    lSf.StartEditingTable()
    'TODO: error handling
    Dim lBsuc As Boolean = lSf.EditDeleteField(aFieldIndex)
    lSf.StopEditingTable()
  End Function

  ''' <summary>Returns the number of layers loaded in the MapWindow. Drawing layers are not counted. </summary>
  Public Shared ReadOnly Property NumLayers() As Integer
    Get
      Return MappingObject.Layers.NumLayers()
    End Get
  End Property

  Public Shared Function LayerType(Optional ByVal aLayerIndex As Integer = UseCurrent) As Integer
    Return LayerFromIndex(aLayerIndex).LayerType
  End Function

  Public Shared Function LayerName(Optional ByVal aLayerIndex As Integer = UseCurrent) As String
    Return LayerFromIndex(aLayerIndex).Name
  End Function

  ''' <summary>Obtain index of a layer from a name</summary>
  ''' <param name="aLayerName">
  '''     <para>Name of layer to obtain index for</para>
  ''' </param>
  Public Shared Function LayerIndex(ByVal aLayerName As String) As Integer
    Dim lLayerIndex As Integer
    Dim lLayer As MapWindow.Interfaces.Layer

    For lLayerIndex = 0 To pMapWin.Layers.NumLayers - 1
      lLayer = pMapWin.Layers(lLayerIndex)
      If UCase(lLayer.Name) = UCase(aLayerName) Then
        Return lLayerIndex
      End If
    Next
    Throw New Exception("GisUtil:LayerIndex:Error:LayerName:" & aLayerName & ":IsNotRecognized")
  End Function

  Public Shared Function LayerFileName(Optional ByVal aLayerIndex As Integer = UseCurrent) As String
    Return LayerFromIndex(aLayerIndex).FileName
  End Function

  Public Shared Function NumFeatures(Optional ByVal aLayerIndex As Integer = UseCurrent) As Integer
    Return ShapeFileFromIndex(aLayerIndex).NumShapes
  End Function

  Public Shared Function FieldType(ByVal aFieldIndex As Integer, Optional ByVal aLayerIndex As Integer = UseCurrent) As Integer
    Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
    If aFieldIndex < 0 Or aFieldIndex >= lSf.NumFields Then
      Throw New Exception("GisUtil:FieldType:Error:FieldIndex:" & aFieldIndex & ":OutOfRange:0:" & lSf.NumFields - 1)
    Else
      Return lSf.Field(aFieldIndex).Type
    End If
  End Function

  Public Shared Function FieldValue(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer, ByVal aFieldIndex As Integer) As String
    Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
    If aFieldIndex < 0 Or aFieldIndex >= lSf.NumFields Then
      Throw New Exception("GisUtil:FieldValue:Error:FieldIndex:" & aFieldIndex & ":OutOfRange:0:" & lSf.NumFields - 1)
    ElseIf aFeatureIndex < 0 Or aFeatureIndex >= lSf.NumShapes Then
      Throw New Exception("GisUtil:FieldValue:Error:FeatureIndex:" & aFeatureIndex & ":OutOfRange:0:" & lSf.NumShapes - 1)
    Else
      Return lSf.CellValue(aFieldIndex, aFeatureIndex)
    End If
  End Function

  Public Shared Function GridLayerMinimum(ByVal aLayerIndex As Integer) As Object
    Return GridFromIndex(aLayerIndex).Minimum
  End Function

  Public Shared Function GridLayerMaximum(ByVal aLayerIndex As Integer) As Object
    Return GridFromIndex(aLayerIndex).Maximum
  End Function

  Public Shared Function FeatureArea(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer) As Double
    Dim lArea As Double

    Try
      Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
      Dim lShape As MapWinGIS.Shape = lSf.Shape(aFeatureIndex)
      Dim lUtils As MapWinGIS.Utils
      lArea = lUtils.Area(lShape)
      If lArea < 0.000001 Then 'could it be undefined?
        If lSf.ShapefileType <> MapWinGIS.ShpfileType.SHP_POLYGON AndAlso _
           lSf.ShapefileType <> MapWinGIS.ShpfileType.SHP_POLYGONM AndAlso _
           lSf.ShapefileType <> MapWinGIS.ShpfileType.SHP_POLYGONZ Then
          lArea = Double.NaN
          LogDbg("GisUtil:FeatureArea:AreaUndefinedForShapeType:" & lSf.ShapefileType)
        End If
      End If
    Catch ex As Exception
      LogDbg("GisUtil:FeatureArea:Exception:" & ex.Message)
      lArea = Double.NaN
    End Try
    Return lArea

  End Function

  Public Shared Function FeatureLength(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer) As Double
    Dim lLength As Double

    Try
      Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
      Dim lShape As MapWinGIS.Shape = lSf.Shape(aFeatureIndex)
      Dim lUtils As MapWinGIS.Utils
      lLength = lUtils.Length(lShape)
      If lLength < 0.000001 Then 'could it be undefined
        Dim lShapeFileType = lSf.ShapefileType
        If lShapeFileType <> MapWinGIS.ShpfileType.SHP_POLYLINE AndAlso _
           lShapeFileType <> MapWinGIS.ShpfileType.SHP_POLYLINEM AndAlso _
           lShapeFileType <> MapWinGIS.ShpfileType.SHP_POLYLINEZ Then
          LogDbg("GisUtil:FeatureLength:LengthUndefinedForShapeType:" & lSf.ShapefileType)
          lLength = Double.NaN
        End If
      End If
    Catch ex As Exception
      LogDbg("GisUtil:FeatureLength:Exception:" & ex.Message)
      lLength = Double.NaN
    End Try
    Return lLength
  End Function

  Public Shared Sub EndPointsOfLine(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer, _
                                    ByRef aX1 As Double, ByRef aY1 As Double, ByRef aX2 As Double, ByRef aY2 As Double)
    Dim sf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
    Dim sfshape As New MapWinGIS.Shape
    sfshape = sf.Shape(aFeatureIndex)
    aX1 = sfshape.Point(0).x
    aY1 = sfshape.Point(0).y
    aX2 = sfshape.Point(sfshape.numPoints - 1).x
    aY2 = sfshape.Point(sfshape.numPoints - 1).y
  End Sub

  Public Shared Sub PointXY(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer, _
                            ByRef aX As Double, ByRef aY As Double)
    Dim sf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
    Dim sfshape As New MapWinGIS.Shape
    sfshape = sf.Shape(aFeatureIndex)
    aX = sfshape.Point(0).x
    aY = sfshape.Point(0).y
  End Sub

  Public Shared Function RemoveFeatureFromLayer(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer) As Double
    Dim sf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
    Dim bsuc As Boolean

    sf.StartEditingShapes(True)
    bsuc = sf.EditDeleteShape(aFeatureIndex)
    sf.StopEditingShapes(True, True)
    'TODO: what is the return value
  End Function

  Public Shared Sub SetValueNthFeatureInLayer(ByVal aLayerIndex As Integer, ByVal aFieldIndex As Integer, ByVal aFeatureIndex As Integer, ByVal aValue As Object)
    Dim sf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
    Dim bsuc As Boolean
    'TODO: error checking
    bsuc = sf.StartEditingTable()
    bsuc = sf.EditCellValue(aFieldIndex, aFeatureIndex, aValue)
    bsuc = sf.StopEditingTable()
  End Sub

  Public Shared Function NumSelectedFeaturesInLayer(ByVal aLayerIndex As Integer) As Integer
    Dim lLayer As MapWindow.Interfaces.Layer
    Dim lSi As MapWindow.Interfaces.SelectInfo

    lSi = pMapWin.View.SelectedShapes
    lLayer = LayerFromIndex(aLayerIndex) 'TODO: what does this do?
    If lSi.LayerHandle = aLayerIndex Then
      NumSelectedFeaturesInLayer = lSi.NumSelected()
    Else
      NumSelectedFeaturesInLayer = 0
    End If
  End Function

  Public Shared Function IndexOfNthSelectedFeatureInLayer(ByVal nth As Integer, ByVal layerindex As Integer) As Integer
    Dim lLayer As MapWindow.Interfaces.Layer
    Dim lsi As MapWindow.Interfaces.SelectInfo

    lsi = pMapWin.View.SelectedShapes
    lLayer = pMapWin.Layers(layerindex)
    If lsi.LayerHandle = layerindex Then
      IndexOfNthSelectedFeatureInLayer = lsi.Item(nth).ShapeIndex
    End If
  End Function

  'Public Shared Sub ExtentsOfLayer(ByVal layerindex As Integer, ByVal xmax As Double, ByVal xmin As Double, ByVal ymax As Double, ByVal ymin As Double)
  '  Dim lLayer As MapWindow.Interfaces.Layer

  '  lLayer = pMapWin.Layers(layerindex)
  '  Dim sf As New MapWinGIS.Shapefile
  '  sf = lLayer.GetObject
  '  xmax = sf.Extents.xMax
  '  xmin = sf.Extents.xMin
  '  ymax = sf.Extents.yMax
  '  ymin = sf.Extents.yMin
  'End Sub

  'Public Shared Function CreateNewShapefile(ByVal basename As String, ByVal type As Integer) As Boolean
  '  Dim osf As New MapWinGIS.Shapefile
  '  'MapWinGIS.ShpfileType.SHP_POLYGON = 5
  '  osf.CreateNew(basename, type)
  'End Function

  Public Shared ReadOnly Property ProjectFileName() As String
    Get
      Return MappingObject.Project.FileName
    End Get
  End Property

  Public Shared Function PointInPolygon(ByVal aPointLayerIndex As Integer, ByVal aPointIndex As Integer, ByVal aPolygonLayerIndex As Integer) As Integer
    'given a point and a polygon layer, return the polygon this point is in
    Dim lPointSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aPointLayerIndex)
    Dim lPolygonSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aPolygonLayerIndex)
    Dim lX As Double, lY As Double

    lPolygonSf.BeginPointInShapefile()
    lX = lPointSf.Shape(aPointIndex - 1).Point(0).x
    lY = lPointSf.Shape(aPointIndex - 1).Point(0).y
    PointInPolygon = lPolygonSf.PointInShapefile(lX, lY)
    lPolygonSf.EndPointInShapefile()
  End Function

  Public Shared Function PointInPolygonXY(ByVal aX As Double, ByVal aY As Double, ByVal aPolygonLayerIndex As Integer) As Integer
    'given a point xy and a polygon layer, return the polygon this point is in
    Dim lPolygonSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aPolygonLayerIndex)

    lPolygonSf.BeginPointInShapefile()
    PointInPolygonXY = lPolygonSf.PointInShapefile(aX, aY)
    lPolygonSf.EndPointInShapefile()
  End Function

  Public Shared Function AddLayerToMap(ByVal aFileName As String, ByVal aLayerName As String) As Boolean
    'given a shape file name, add it to the map.
    'return true if the layer is already there or successfully added.

    If LayerIndex(aLayerName) > -1 Then 'already on map 
      AddLayerToMap = True
    Else
      Dim lLayer As MapWindow.Interfaces.Layer = MappingObject.Layers.Add(aFileName, aLayerName)
      If lLayer Is Nothing Then
        AddLayerToMap = False
      Else
        lLayer.Visible = False
        AddLayerToMap = True
      End If
    End If
  End Function

  Public Shared Property LayerVisible(ByVal aLayerIndex As Integer) As Boolean
    Get
      Return MappingObject.Layers(aLayerIndex).Visible
    End Get
    Set(ByVal aNewValue As Boolean)
      MappingObject.Layers(aLayerIndex).Visible = aNewValue
    End Set
  End Property

  Public Shared Function RemoveLayerFromMap(ByVal aLayerIndex As Integer) As Boolean
    'given a layer index, remove it from the map.
    MappingObject.Layers.Remove(aLayerIndex)
  End Function

  Public Shared Sub TabulateAreas(ByVal gridLayerIndex As Integer, _
                                  ByVal polygonLayerIndex As Integer, _
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
    Dim gridLayer As MapWindow.Interfaces.Layer
    Dim polygonLayer As MapWindow.Interfaces.Layer

    'set input grid
    gridLayer = MappingObject.Layers(gridLayerIndex)
    Dim InputGrid As New MapWinGIS.Grid
    InputGrid = gridLayer.GetGridObject

    'set input polygon layer
    polygonLayer = MappingObject.Layers(polygonLayerIndex)
    Dim polygonsf As New MapWinGIS.Shapefile
    polygonsf = polygonLayer.GetObject

    'figure out what part of the grid overlays these polygons
    InputGrid.ProjToCell(polygonsf.Extents.xMin, polygonsf.Extents.yMin, startingcolumn, endingrow)
    InputGrid.ProjToCell(polygonsf.Extents.xMax, polygonsf.Extents.yMax, endingcolumn, startingrow)

    lcellarea = InputGrid.Header.dX * InputGrid.Header.dY
    'totalcellcount = (endingcolumn - startingcolumn) * (endingrow - startingrow)
    'cellcount = 0
    'lastdisplayed = 0

    polygonsf.BeginPointInShapefile()
    For ic = startingcolumn To endingcolumn
      For ir = startingrow To endingrow
        InputGrid.CellToProj(ic, ir, xpos, ypos)
        subid = polygonsf.PointInShapefile(xpos, ypos)
        If subid > -1 Then
          'this is in a subbasin
          luid = InputGrid.Value(ic, ir)
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
    polygonsf.EndPointInShapefile()
  End Sub

  Public Shared Sub GridMinMaxInPolygon(ByVal gridLayerIndex As Integer, ByVal polygonLayerIndex As Integer, _
    ByVal polygonfeatureindex As Integer, ByVal Min As Double, ByVal Max As Double)
    'Given a grid and a polygon layer, find the min and max grid value within the feature.

    Dim ic As Integer
    Dim ir As Integer
    Dim xpos As Double
    Dim ypos As Double
    Dim subid As Integer
    Dim startingcolumn As Integer
    Dim endingcolumn As Integer
    Dim startingrow As Integer
    Dim endingrow As Integer
    Dim gridLayer As MapWindow.Interfaces.Layer
    Dim polygonLayer As MapWindow.Interfaces.Layer
    Dim val As Integer

    'set input grid
    gridLayer = MappingObject.Layers(gridLayerIndex)
    Dim InputGrid As New MapWinGIS.Grid
    InputGrid = gridLayer.GetGridObject

    'set input polygon layer
    polygonLayer = MappingObject.Layers(polygonLayerIndex)
    Dim polygonsf As New MapWinGIS.Shapefile
    polygonsf = polygonLayer.GetObject
    Dim pshape As New MapWinGIS.Shape
    pshape = polygonsf.Shape(polygonfeatureindex)

    'figure out what part of the grid overlays this polygon
    InputGrid.ProjToCell(pshape.Extents.xMin, pshape.Extents.yMin, startingcolumn, endingrow)
    InputGrid.ProjToCell(pshape.Extents.xMax, pshape.Extents.yMax, endingcolumn, startingrow)

    Min = 99999999
    Max = -99999999
    polygonsf.BeginPointInShapefile()
    For ic = startingcolumn To endingcolumn
      For ir = startingrow To endingrow
        InputGrid.CellToProj(ic, ir, xpos, ypos)
        subid = polygonsf.PointInShapefile(xpos, ypos)
        If subid = polygonfeatureindex Then
          'this is in the polygon we want
          val = InputGrid.Value(ic, ir)
          If val > Max Then
            Max = val
          End If
          If val < Min Then
            Min = val
          End If
        End If
      Next ir
    Next ic
    polygonsf.EndPointInShapefile()
  End Sub

  Public Shared Function GridValueAtPoint(ByVal GridLayerIndex As Integer, ByVal x As Double, ByVal y As Double) As Integer
    Dim column As Integer
    Dim row As Integer
    Dim endingrow As Integer
    Dim gridLayer As MapWindow.Interfaces.Layer

    'set input grid
    gridLayer = MappingObject.Layers(GridLayerIndex)
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
    lLayer = MappingObject.Layers(Layer1Index)
    Dim lusf As New MapWinGIS.Shapefile
    lusf = lLayer.GetObject

    'set layer 2 (subbasins)
    Layer2Index = LayerIndex(Layer2Name)
    Layer2FieldIndex = FieldIndex(Layer2Index, Layer2FieldName)
    Dim sLayer As MapWindow.Interfaces.Layer
    sLayer = MappingObject.Layers(Layer2Index)
    Dim ssf As New MapWinGIS.Shapefile
    ssf = sLayer.GetObject
    SSFext = ssf.Extents

    'if any of layer 2 is selected, use only those
    Dim cSelectedSubbasins As New Collection
    For i = 1 To NumSelectedFeaturesInLayer(Layer2Index)
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

    '********** do overlay ***********
    MappingObject.StatusBar.ShowProgressBar = True
    totalpolygoncount = lusf.NumShapes * cSelectedSubbasins.Count
    polygoncount = 0
    lastdisplayed = 0
    MappingObject.StatusBar.ProgressBarValue = 0
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
          'look for intersection from overlay of these shapes
          newshape = utilClip.ClipPolygon(MapWinGIS.PolygonOperation.INTERSECTION_OPERATION, lusfshape, ssf.Shape(shapeindex))
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
        Next k
      Else
        polygoncount = polygoncount + cSelectedSubbasins.Count
      End If
      If Int(polygoncount / totalpolygoncount * 100) > lastdisplayed Then
        'lblStatus.Text = "Overlaying Land Use and Subbasins (" & Int(polygoncount / totalpolygoncount * 100) & "%)"
        'Me.Refresh()
        lastdisplayed = Int(polygoncount / totalpolygoncount * 100)
        MappingObject.StatusBar.ProgressBarValue = Int(polygoncount / totalpolygoncount * 100)
      End If
    Next i
    MappingObject.StatusBar.ShowProgressBar = False

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
    Dim lLayer As MapWindow.Interfaces.Layer
    lLayer = MappingObject.Layers(inputlayerindex)
    Dim isf As New MapWinGIS.Shapefile
    isf = lLayer.GetObject

    'set clipper layer (subbasins for instance)
    Dim sLayer As MapWindow.Interfaces.Layer
    sLayer = MappingObject.Layers(clipperlayerindex)
    Dim ssf As New MapWinGIS.Shapefile
    ssf = sLayer.GetObject
    Dim sshape As MapWinGIS.Shape
    Dim clipperpath As String
    clipperpath = PathNameOnly(LayerFileName(clipperlayerindex))

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

    MappingObject.StatusBar.ShowProgressBar = True
    For i = 1 To ssf.NumShapes
      MappingObject.StatusBar.ProgressBarValue = Int(i / ssf.NumShapes * 100)
      sshape = ssf.Shape(i - 1)
      bsuc = mx.ClipShapesWithPolygon(isf, sshape, rsf)
      For j = 1 To rsf.NumShapes
        bsuc = orsf.EditInsertShape(rsf.Shape(j - 1), j - 1)
      Next j
    Next i
    MappingObject.StatusBar.ShowProgressBar = False

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

  Public Shared Sub MergeFeaturesBasedOnAttribute(ByVal LayerIndex, ByVal FieldIndex)
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

    Dim lLayer As MapWindow.Interfaces.Layer
    lLayer = MappingObject.Layers(LayerIndex)
    Dim isf As New MapWinGIS.Shapefile
    isf = lLayer.GetObject

    isf.StartEditingShapes(True)
    'merge together based on common endpoints
    i = 0
    Do While i < isf.NumShapes
      found = False
      shape1 = isf.Shape(i)
      targetval = FieldValue(LayerIndex, i, FieldIndex)
      endx = shape1.Point(shape1.numPoints - 1).x
      endy = shape1.Point(shape1.numPoints - 1).y
      j = 0
      Do While j < isf.NumShapes
        If i <> j Then
          shape2 = isf.Shape(j)
          thisval = FieldValue(LayerIndex, j, FieldIndex)
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
      targetval = FieldValue(LayerIndex, i, FieldIndex)
      startx1 = shape1.Point(0).x
      starty1 = shape1.Point(0).y
      endx1 = shape1.Point(shape1.numPoints - 1).x
      endy1 = shape1.Point(shape1.numPoints - 1).y
      j = 0
      Do While j < isf.NumShapes
        If i <> j Then
          shape2 = isf.Shape(j)
          thisval = FieldValue(LayerIndex, j, FieldIndex)
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
