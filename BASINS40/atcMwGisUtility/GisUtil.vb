Imports atcUtility

Public Class GisUtil

  'GIS Utilities Implemented thru MapWindow
  Private Shared pMapWin As MapWindow.Interfaces.IMapWin
  Private Const UseCurrent As Integer = -1

  Public Shared Function RunAllTests() As Boolean
    Dim lTests As New Test_GisUtil
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

  Public Shared Property MappingObject() As MapWindow.Interfaces.IMapWin
    Get
      Return pMapWin
    End Get
    Set(ByVal aNewValue As MapWindow.Interfaces.IMapWin)
      Try
        pMapWin = aNewValue
      Catch e As Exception
        LogDbg("GisUtil:SetGisUtilsMappingObject:Error:" & e.Message)
      End Try
    End Set
  End Property

  Public Shared Sub LoadProject(ByVal aProjectName As String)
    If FileExists(aProjectName) Then
      Dim lBaseDir As String = CurDir()  'dont want to change curdir, save original
      Dim lRet As Boolean = pMapWin.Project.Load(aProjectName)
      ChDriveDir(lBaseDir)
    End If
    'TODO: error return?
  End Sub

  Public Shared Property CurrentLayer() As Integer
    Get
      Return pMapWin.Layers.CurrentLayer
    End Get
    Set(ByVal aNewValue As Integer)
      If aNewValue < pMapWin.Layers.NumLayers And aNewValue >= 0 Then
        pMapWin.Layers.CurrentLayer = aNewValue
      Else
        'TODO: need an error here
      End If
    End Set
  End Property

  Private Shared Function ShapeFileFromIndex(Optional ByVal aLayerIndex As Integer = UseCurrent) As MapWinGIS.Shapefile
    If aLayerIndex = UseCurrent Then aLayerIndex = CurrentLayer

    If aLayerIndex >= 0 And aLayerIndex < pMapWin.Layers.NumLayers Then
      Dim lLayer As MapWindow.Interfaces.Layer = pMapWin.Layers(aLayerIndex)
      If lLayer.LayerType = MapWindow.Interfaces.eLayerType.LineShapefile OrElse _
         lLayer.LayerType = MapWindow.Interfaces.eLayerType.PointShapefile OrElse _
         lLayer.LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
        Dim lShape As MapWinGIS.Shapefile = (lLayer.GetObject)
        Return lShape
      End If
    Else
      'TODO: need an error here
    End If
    Return Nothing
  End Function

  Private Shared Function LayerFromIndex(Optional ByVal aLayerIndex As Integer = UseCurrent) As MapWindow.Interfaces.Layer
    If aLayerIndex = UseCurrent Then aLayerIndex = CurrentLayer

    If aLayerIndex >= 0 And aLayerIndex < pMapWin.Layers.NumLayers Then
      Return (pMapWin.Layers(aLayerIndex))
    Else
      'TODO: need an error here
    End If
  End Function

  Public Shared Function OverlappingPolygons( _
    ByVal aLayerIndex1 As Integer, ByVal aFeatureIndex1 As Integer, _
    ByVal aLayerIndex2 As Integer, ByVal aFeatureIndex2 As Integer) As Boolean

    Dim lIndex As Integer
    Dim lX As Double, lY As Double

    Dim lSf1 As MapWinGIS.Shapefile = LayerFromIndex(aLayerIndex1).GetObject
    Dim lSf1Shape As MapWinGIS.Shape = lSf1.Shape(aFeatureIndex1)

    Dim lSf2 As MapWinGIS.Shapefile = LayerFromIndex(aLayerIndex2).GetObject
    Dim lSf2Shape As MapWinGIS.Shape = lSf2.Shape(aFeatureIndex2)

    OverlappingPolygons = False
    If lSf1Shape Is Nothing OrElse lSf2Shape Is Nothing Then
      'one shape undefined, no overlap
    Else 'quick - check extents
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
  End Function

  Public Shared Sub AssignContainingPolygons( _
    ByVal aLayerIndex As Integer, _
    ByVal aLayerIndexContaining As Integer, _
    ByRef aIndex() As Integer)
    'given a polygon layer (like dem shape) and a containing layer (like subbasins),
    'return which polygon in the containing layer each polygon lies within

    Dim lPointIndex As Integer
    Dim lShapeIndex As Integer
    Dim lX As Double, lY As Double

    Dim lSf1 As MapWinGIS.Shapefile = LayerFromIndex(aLayerIndex).GetObject
    Dim lSf1Shape As New MapWinGIS.Shape
    Dim lSf2 As MapWinGIS.Shapefile = LayerFromIndex(aLayerIndexContaining).GetObject
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
        If lNth > -1 Then
          aIndex(lShapeIndex) = lNth
          Exit For
        End If
      Next lPointIndex
    Next lShapeIndex
    lSf2.EndPointInShapefile()

  End Sub

  Public Shared Function NumFields(Optional ByVal aLayerIndex As Integer = UseCurrent) As Integer
    Dim lSf As MapWinGIS.Shapefile = LayerFromIndex(aLayerIndex).GetObject
    Return lSf.NumFields
  End Function

  Public Shared Function FieldName(ByVal aFieldIndex As Integer, ByVal aLayerIndex As Integer) As String
    Dim lSf As MapWinGIS.Shapefile = LayerFromIndex(aLayerIndex).GetObject
    Return lSf.Field(aFieldIndex).Name
  End Function

  Public Shared Function LayerIndex(ByVal aLayerName As String) As Integer
    Dim lLayerIndex As Integer, lLayer As MapWindow.Interfaces.Layer

    LayerIndex = -1
    For lLayerIndex = 0 To pMapWin.Layers.NumLayers - 1
      lLayer = pMapWin.Layers(lLayerIndex)
      If UCase(lLayer.Name) = UCase(aLayerName) Then
        LayerIndex = lLayerIndex
        Exit For
      End If
    Next
  End Function

  Public Shared Function FieldIndex(ByVal aLayerIndex As Integer, ByVal aFieldName As String) As Integer
    Dim lSf As MapWinGIS.Shapefile = LayerFromIndex(aLayerIndex).GetObject

    FieldIndex = -1
    For iFieldIndex As Integer = 0 To lSf.NumFields - 1
      If UCase(lSf.Field(iFieldIndex).Name) = UCase(aFieldName) Then 'this is the field we want
        FieldIndex = iFieldIndex
        Exit For
      End If
    Next
  End Function

  Public Shared Function AddField(ByVal aLayerIndex As Integer, ByVal aFieldName As String, _
                                  ByVal aFieldType As Integer, ByVal aFieldWidth As Integer) As Integer
    Dim lField As New MapWinGIS.Field
    lField.Name = aFieldName
    lField.Type = aFieldType
    lField.Width = aFieldWidth

    Dim lBsuc As Boolean
    Dim lSf As MapWinGIS.Shapefile = LayerFromIndex(aLayerIndex).GetObject

    lSf.StartEditingTable()
    lBsuc = lSf.EditInsertField(lField, lSf.NumFields)
    'TODO: error handling
    lSf.StopEditingTable()
    Return lSf.NumFields - 1
  End Function

  Public Shared Function RemoveField(ByVal aLayerIndex As Integer, ByVal aFieldIndex As Integer)
    Dim lBsuc As Boolean
    Dim lSf As MapWinGIS.Shapefile = LayerFromIndex(aLayerIndex).GetObject

    lSf.StartEditingTable()
    lBsuc = lSf.EditDeleteField(aFieldIndex)
    'TODO: error handling
    lSf.StopEditingTable()
  End Function

  Public Shared Function NumLayers() As Integer
    Return pMapWin.Layers.NumLayers()
  End Function

  Public Shared Function LayerType(Optional ByVal aLayerIndex As Integer = UseCurrent) As Integer
    Return LayerFromIndex(aLayerIndex).LayerType
  End Function

  Public Shared Function LayerName(Optional ByVal aLayerIndex As Integer = UseCurrent) As String
    Return LayerFromIndex(aLayerIndex).Name
  End Function

  Public Shared Function LayerFileName(Optional ByVal aLayerIndex As Integer = UseCurrent) As String
    Return LayerFromIndex(aLayerIndex).FileName
  End Function

  Public Shared Function NumFeatures(Optional ByVal aLayerIndex As Integer = UseCurrent) As Integer
    Dim lSf As MapWinGIS.Shapefile = ShapeFileFromIndex(aLayerIndex)
    If lSf Is Nothing Then
      Return 0
    Else
      Return lSf.NumShapes
    End If
  End Function

  Public Shared Function FieldType(ByVal aFieldIndex As Integer, ByVal aLayerIndex As Integer) As Integer
    Dim lSf As MapWinGIS.Shapefile = LayerFromIndex(aLayerIndex).GetObject
    Return lSf.Field(aFieldIndex).Type
  End Function

  Public Shared Function FieldValue(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer, ByVal aFieldIndex As Integer) As String
    Dim lSf As MapWinGIS.Shapefile = LayerFromIndex(aLayerIndex).GetObject
    Dim lFieldValue As String = lSf.CellValue(aFieldIndex, aFeatureIndex)
    Return lFieldValue
  End Function

  Public Shared Function GridLayerMinimum(ByVal aLayerIndex As Integer) As Object
    Dim lLayer As MapWindow.Interfaces.Layer = LayerFromIndex(aLayerIndex)
    Dim lUgrid As New MapWinGIS.Grid
    lUgrid = lLayer.GetGridObject
    Return lUgrid.Minimum
  End Function

  Public Shared Function GridLayerMaximum(ByVal aLayerIndex As Integer) As Object
    Dim lLayer As MapWindow.Interfaces.Layer = LayerFromIndex(aLayerIndex)
    Dim lUgrid As New MapWinGIS.Grid
    lUgrid = lLayer.GetGridObject
    Return lUgrid.Maximum
  End Function

  Public Shared Function FeatureArea(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer) As Double
    Dim lSf As MapWinGIS.Shapefile = LayerFromIndex(aLayerIndex).GetObject

    Dim lShape As New MapWinGIS.Shape
    lShape = lSf.Shape(aFeatureIndex)
    Dim lUtils As New MapWinGIS.Utils
    Dim lArea As Double = lUtils.Area(lShape)
    If lArea < 0.000001 Then 'could it be undefined?
      Dim lShapeFileType = lSf.ShapefileType
      If lShapeFileType <> MapWinGIS.ShpfileType.SHP_POLYGON AndAlso _
         lShapeFileType <> MapWinGIS.ShpfileType.SHP_POLYGONM AndAlso _
         lShapeFileType <> MapWinGIS.ShpfileType.SHP_POLYGONZ Then
        lArea = Double.NaN
      End If
    End If
    Return lArea
  End Function

  Public Shared Function FeatureLength(ByVal aLayerIndex As Integer, ByVal aFeatureIndex As Integer) As Double
    Dim lSf As MapWinGIS.Shapefile = LayerFromIndex(aLayerIndex).GetObject
    Dim lshape As New MapWinGIS.Shape
    lshape = lSf.Shape(aFeatureIndex)
    Dim lUtils As New MapWinGIS.Utils
    Dim lLength As Double = lUtils.Length(lshape)
    If lLength < 0.000001 Then 'could it be undefined
      Dim lShapeFileType = lSf.ShapefileType
      If lShapeFileType <> MapWinGIS.ShpfileType.SHP_POLYLINE AndAlso _
         lShapeFileType <> MapWinGIS.ShpfileType.SHP_POLYLINEM AndAlso _
         lShapeFileType <> MapWinGIS.ShpfileType.SHP_POLYLINEZ Then
        lLength = Double.NaN
      End If
    End If
    Return lLength
  End Function

  Public Shared Sub EndPointsOfLine(ByVal layerindex As Integer, ByVal featureindex As Integer, ByRef x1 As Double, ByRef y1 As Double, ByRef x2 As Double, ByRef y2 As Double)
    Dim lLayer As MapWindow.Interfaces.Layer

    lLayer = pMapWin.Layers(layerindex)
    Dim sf As New MapWinGIS.Shapefile
    sf = lLayer.GetObject
    Dim sfshape As New MapWinGIS.Shape
    sfshape = sf.Shape(featureindex)
    x1 = sfshape.Point(0).x
    y1 = sfshape.Point(0).y
    x2 = sfshape.Point(sfshape.numPoints - 1).x
    y2 = sfshape.Point(sfshape.numPoints - 1).y
  End Sub

  Public Shared Sub PointXY(ByVal layerindex As Integer, ByVal featureindex As Integer, ByRef x As Double, ByRef y As Double)
    Dim lLayer As MapWindow.Interfaces.Layer

    lLayer = pMapWin.Layers(layerindex)
    Dim sf As New MapWinGIS.Shapefile
    sf = lLayer.GetObject
    Dim sfshape As New MapWinGIS.Shape
    sfshape = sf.Shape(featureindex)
    x = sfshape.Point(0).x
    y = sfshape.Point(0).y
  End Sub

  Public Shared Function RemoveFeatureFromLayer(ByVal layerindex As Integer, ByVal featureindex As Integer) As Double
    Dim lLayer As MapWindow.Interfaces.Layer
    Dim bsuc As Boolean

    lLayer = pMapWin.Layers(layerindex)
    Dim sf As New MapWinGIS.Shapefile
    sf = lLayer.GetObject
    sf.StartEditingShapes(True)
    bsuc = sf.EditDeleteShape(featureindex)
    sf.StopEditingShapes(True, True)
  End Function

  Public Shared Sub SetValueNthFeatureInLayer(ByVal layerindex As Integer, ByVal fieldindex As Integer, ByVal featureindex As Integer, ByVal value As Object)
    Dim lLayer As MapWindow.Interfaces.Layer
    Dim bsuc As Boolean

    lLayer = pMapWin.Layers(layerindex)
    Dim sf As MapWinGIS.Shapefile
    sf = lLayer.GetObject

    bsuc = sf.StartEditingTable()
    bsuc = sf.EditCellValue(fieldindex, featureindex, value)
    bsuc = sf.StopEditingTable()
  End Sub

  Public Shared Function NumSelectedFeaturesInLayer(ByVal layerindex As Integer) As Integer
    Dim lLayer As MapWindow.Interfaces.Layer
    Dim lsi As MapWindow.Interfaces.SelectInfo

    lsi = pMapWin.View.SelectedShapes
    lLayer = pMapWin.Layers(layerindex)
    If lsi.LayerHandle = layerindex Then
      NumSelectedFeaturesInLayer = lsi.NumSelected()
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
      Return pMapWin.Project.FileName
    End Get
  End Property

  Public Shared Function PointInPolygon(ByVal PointLayerIndex As Integer, ByVal nthPoint As Integer, ByVal PolygonLayerIndex As Integer) As Integer
    'given a point and a polygon layer, return the polygon this point is in
    Dim lLayer As MapWindow.Interfaces.Layer
    Dim pointsf As New MapWinGIS.Shapefile
    Dim lLayer2 As MapWindow.Interfaces.Layer
    Dim polygonsf As New MapWinGIS.Shapefile
    Dim x As Double, y As Double

    lLayer = pMapWin.Layers(PointLayerIndex)
    pointsf = lLayer.GetObject
    lLayer2 = pMapWin.Layers(PolygonLayerIndex)
    polygonsf = lLayer2.GetObject

    polygonsf.BeginPointInShapefile()
    x = pointsf.Shape(nthPoint - 1).Point(0).x
    y = pointsf.Shape(nthPoint - 1).Point(0).y
    PointInPolygon = polygonsf.PointInShapefile(x, y)
    polygonsf.EndPointInShapefile()
  End Function

  Public Shared Function PointInPolygonXY(ByVal x As Double, ByVal y As Double, ByVal PolygonLayerIndex As Integer) As Integer
    'given a point xy and a polygon layer, return the polygon this point is in
    Dim lLayer As MapWindow.Interfaces.Layer
    Dim polygonsf As New MapWinGIS.Shapefile

    lLayer = pMapWin.Layers(PolygonLayerIndex)
    polygonsf = lLayer.GetObject

    polygonsf.BeginPointInShapefile()
    PointInPolygonXY = polygonsf.PointInShapefile(x, y)
    polygonsf.EndPointInShapefile()
  End Function

  Public Shared Function AddLayerToMap(ByVal newfilename As String, ByVal layername As String) As Boolean
    'given a shape file name, add it to the map.
    'return true if the layer is already there or successfully added.
    Dim lLayer As MapWindow.Interfaces.Layer

    If LayerIndex(layername) > -1 Then
      'already on map 
      AddLayerToMap = True
    Else
      lLayer = pMapWin.Layers.Add(newfilename, layername)
      If lLayer Is Nothing Then
        AddLayerToMap = False
      Else
        lLayer.Visible = False
        AddLayerToMap = True
      End If
    End If
  End Function

  Public Shared Property LayerVisible(ByVal aIndex As Integer) As Boolean
    Get
      Return pMapWin.Layers(aIndex).Visible
    End Get
    Set(ByVal aNewValue As Boolean)
      pMapWin.Layers(aIndex).Visible = aNewValue
    End Set
  End Property

  Public Shared Function RemoveLayerFromMap(ByVal layerindex As Integer) As Boolean
    'given a layer index, remove it from the map.
    pMapWin.Layers.Remove(layerindex)
  End Function

  Public Shared Sub TabulateAreas(ByVal gridLayerIndex As Integer, ByVal polygonLayerIndex As Integer, _
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
    gridLayer = pMapWin.Layers(gridLayerIndex)
    Dim InputGrid As New MapWinGIS.Grid
    InputGrid = gridLayer.GetGridObject

    'set input polygon layer
    polygonLayer = pMapWin.Layers(polygonLayerIndex)
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
    gridLayer = pMapWin.Layers(gridLayerIndex)
    Dim InputGrid As New MapWinGIS.Grid
    InputGrid = gridLayer.GetGridObject

    'set input polygon layer
    polygonLayer = pMapWin.Layers(polygonLayerIndex)
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
    gridLayer = pMapWin.Layers(GridLayerIndex)
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
    lLayer = pMapWin.Layers(Layer1Index)
    Dim lusf As New MapWinGIS.Shapefile
    lusf = lLayer.GetObject

    'set layer 2 (subbasins)
    Layer2Index = LayerIndex(Layer2Name)
    Layer2FieldIndex = FieldIndex(Layer2Index, Layer2FieldName)
    Dim sLayer As MapWindow.Interfaces.Layer
    sLayer = pMapWin.Layers(Layer2Index)
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
    pMapWin.StatusBar.ShowProgressBar = True
    totalpolygoncount = lusf.NumShapes * cSelectedSubbasins.Count
    polygoncount = 0
    lastdisplayed = 0
    pMapWin.StatusBar.ProgressBarValue = 0
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
        pMapWin.StatusBar.ProgressBarValue = Int(polygoncount / totalpolygoncount * 100)
      End If
    Next i
    pMapWin.StatusBar.ShowProgressBar = False

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
    lLayer = pMapWin.Layers(inputlayerindex)
    Dim isf As New MapWinGIS.Shapefile
    isf = lLayer.GetObject

    'set clipper layer (subbasins for instance)
    Dim sLayer As MapWindow.Interfaces.Layer
    sLayer = pMapWin.Layers(clipperlayerindex)
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

    pMapWin.StatusBar.ShowProgressBar = True
    For i = 1 To ssf.NumShapes
      pMapWin.StatusBar.ProgressBarValue = Int(i / ssf.NumShapes * 100)
      sshape = ssf.Shape(i - 1)
      bsuc = mx.ClipShapesWithPolygon(isf, sshape, rsf)
      For j = 1 To rsf.NumShapes
        bsuc = orsf.EditInsertShape(rsf.Shape(j - 1), j - 1)
      Next j
    Next i
    pMapWin.StatusBar.ShowProgressBar = False

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
    lLayer = pMapWin.Layers(LayerIndex)
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
