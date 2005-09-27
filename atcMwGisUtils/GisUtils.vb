Imports atcUtility

Module GISUtils

  'GIS Utilities Implemented thru MapWindow
  Public pMapWin As MapWindow.Interfaces.IMapWin

  Public Sub SetGisUtilsMappingObject(ByVal o As Object)
    pMapWin = o
  End Sub

  Public Function GisUtil_OverlappingPolygons(ByVal layerindex1 As Long, _
    ByVal nthfeature1 As Long, ByVal layerindex2 As Long, _
    ByVal nthfeature2 As Long) As Boolean
    Dim k As Long
    Dim x As Double, y As Double

    Dim lLayer1 As MapWindow.Interfaces.Layer
    Dim sf1 As New MapWinGIS.Shapefile
    Dim sf1shape As New MapWinGIS.Shape
    Dim lLayer2 As MapWindow.Interfaces.Layer
    Dim sf2 As New MapWinGIS.Shapefile
    Dim sf2shape As New MapWinGIS.Shape

    lLayer1 = pMapWin.Layers(layerindex1)
    sf1 = lLayer1.GetObject
    sf1shape = sf1.Shape(nthfeature1)
    lLayer2 = pMapWin.Layers(layerindex2)
    sf2 = lLayer2.GetObject
    sf2shape = sf2.Shape(nthfeature2)

    GisUtil_OverlappingPolygons = False
    If Not (sf1.Extents.xMin > sf2.Extents.xMax Or sf1.Extents.xMax < sf2.Extents.xMin Or sf1.Extents.yMin > sf2.Extents.yMax Or sf1.Extents.yMax < sf2.Extents.yMin) Then
      'check if one inside the other
      For k = 1 To sf1shape.numPoints
        x = sf1shape.Point(k - 1).x
        y = sf1shape.Point(k - 1).y
        GisUtil_OverlappingPolygons = sf2.PointInShape(nthfeature2, x, y)
        If GisUtil_OverlappingPolygons Then
          'quit if we've found they overlap
          Exit For
        End If
      Next k
      If Not GisUtil_OverlappingPolygons Then
        'now check the opposite
        For k = 1 To sf2shape.numPoints
          x = sf2shape.Point(k - 1).x
          y = sf2shape.Point(k - 1).y
          GisUtil_OverlappingPolygons = sf1.PointInShape(nthfeature1, x, y)
          If GisUtil_OverlappingPolygons Then
            'quit if we've found they overlap
            Exit For
          End If
        Next k
      End If
    End If
  End Function

  Public Function GisUtil_AssignContainingPolygons(ByVal layerindex As Long, _
    ByVal layerindexcontaining As Long, ByVal aIndex() As Long)
    'given a polygon layer (like dem shape) and a containing layer (like subbasins),
    'return which polygon in the containing layer each polygon lies within

    Dim k As Long
    Dim i As Long
    Dim x As Double, y As Double
    Dim lLayer1 As MapWindow.Interfaces.Layer
    Dim sf1 As New MapWinGIS.Shapefile
    Dim sf1shape As New MapWinGIS.Shape
    Dim lLayer2 As MapWindow.Interfaces.Layer
    Dim sf2 As New MapWinGIS.Shapefile
    Dim assigned As Boolean
    Dim nth As Integer

    lLayer1 = pMapWin.Layers(layerindex)
    sf1 = lLayer1.GetObject
    lLayer2 = pMapWin.Layers(layerindexcontaining)
    sf2 = lLayer2.GetObject

    sf2.BeginPointInShapefile()
    For i = 1 To sf1.NumShapes
      assigned = False
      sf1shape = sf1.Shape(i - 1)
      For k = 2 To sf1shape.numPoints
        x = sf1shape.Point(k - 1).x
        y = sf1shape.Point(k - 1).y
        nth = sf2.PointInShapefile(x, y)
        If nth > -1 Then
          aIndex(i) = nth
          assigned = True
          Exit For
        End If
      Next k
      If Not assigned Then
        aIndex(i) = -1
      End If
    Next i
    sf2.EndPointInShapefile()

  End Function

  Public Function GisUtil_NumFieldsInLayer(ByVal layerindex As Long) As Long
    Dim lLayer As MapWindow.Interfaces.Layer

    lLayer = pMapWin.Layers(layerindex)
    Dim sf As New MapWinGIS.Shapefile
    sf = lLayer.GetObject
    GisUtil_NumFieldsInLayer = sf.NumFields
  End Function

  Public Function GisUtil_NthFieldNameInLayer(ByVal nth As Long, ByVal layerindex As Long) As String
    Dim lLayer As MapWindow.Interfaces.Layer

    lLayer = pMapWin.Layers(layerindex)
    Dim sf As New MapWinGIS.Shapefile
    sf = lLayer.GetObject
    GisUtil_NthFieldNameInLayer = sf.Field(nth).Name
  End Function

  Public Function GisUtil_FindLayerIndexByName(ByVal layername As String) As Long
    Dim lyr As Long, lLayer As MapWindow.Interfaces.Layer

    GisUtil_FindLayerIndexByName = -1
    For lyr = 0 To pMapWin.Layers.NumLayers - 1
      lLayer = pMapWin.Layers(lyr)
      If UCase(lLayer.Name) = UCase(layername) Then
        GisUtil_FindLayerIndexByName = lyr
        Exit For
      End If
    Next
  End Function

  Public Function GisUtil_FindFieldIndexByName(ByVal layerindex As Long, ByVal fieldname As String) As Long
    Dim i As Long, findex As Long, lLayer As MapWindow.Interfaces.Layer

    GisUtil_FindFieldIndexByName = -1
    lLayer = pMapWin.Layers(layerindex)
    Dim sf As New MapWinGIS.Shapefile
    sf = lLayer.GetObject

    For i = 0 To sf.NumFields - 1
      If sf.Field(i).Name = fieldname Then
        'this is the field we want
        GisUtil_FindFieldIndexByName = i
        Exit For
      End If
    Next
  End Function

  Public Function GisUtil_AddField(ByVal layerindex As Long, ByVal fieldname As String, ByVal fieldtype As Integer, ByVal fieldwidth As Integer) As Long
    Dim i As Long, findex As Long, lLayer As MapWindow.Interfaces.Layer
    Dim bsuc As Boolean

    GisUtil_AddField = -1
    lLayer = pMapWin.Layers(layerindex)
    Dim sf As New MapWinGIS.Shapefile
    sf = lLayer.GetObject
    Dim of As New MapWinGIS.Field
    of.Name = fieldname
    of.Type = fieldtype
    of.Width = fieldwidth
    sf.StartEditingTable()
    bsuc = sf.EditInsertField(of, sf.NumFields)
    sf.StopEditingTable()
    GisUtil_AddField = sf.NumFields - 1
  End Function

  Public Function GisUtil_RemoveField(ByVal layerindex As Long, ByVal fieldindex As Long)
    Dim lLayer As MapWindow.Interfaces.Layer
    Dim bsuc As Boolean

    lLayer = pMapWin.Layers(layerindex)
    Dim sf As New MapWinGIS.Shapefile
    sf = lLayer.GetObject
    sf.StartEditingTable()
    bsuc = sf.EditDeleteField(fieldindex)
    sf.StopEditingTable()
  End Function

  Public Function GisUtil_NumLayers() As Long
    GisUtil_NumLayers = pMapWin.Layers.NumLayers()
  End Function

  Public Function GisUtil_LayerType(ByVal layerindex As Long) As Long
    GisUtil_LayerType = pMapWin.Layers(layerindex).LayerType
  End Function

  Public Function GisUtil_LayerName(ByVal layerindex As Long) As String
    GisUtil_LayerName = pMapWin.Layers(layerindex).Name
  End Function

  Public Function GisUtil_LayerFileName(ByVal layerindex As Long) As String
    GisUtil_LayerFileName = pMapWin.Layers(layerindex).FileName
  End Function

  Public Function GisUtil_NthFieldTypeInLayer(ByVal nth As Long, ByVal layerindex As Long) As Long
    Dim lLayer As MapWindow.Interfaces.Layer

    lLayer = pMapWin.Layers(layerindex)
    Dim sf As New MapWinGIS.Shapefile
    sf = lLayer.GetObject
    GisUtil_NthFieldTypeInLayer = sf.Field(nth).Type
  End Function

  Public Function GisUtil_GridLayerMinimum(ByVal layerindex As Long) As Object
    Dim lLayer As MapWindow.Interfaces.Layer
    Dim lugrid As New MapWinGIS.Grid

    lLayer = pMapWin.Layers(layerindex)
    lugrid = lLayer.GetGridObject

    GisUtil_GridLayerMinimum = lugrid.Minimum
  End Function

  Public Function GisUtil_GridLayerMaximum(ByVal layerindex As Long) As Object
    Dim lLayer As MapWindow.Interfaces.Layer
    Dim lugrid As New MapWinGIS.Grid

    lLayer = pMapWin.Layers(layerindex)
    lugrid = lLayer.GetGridObject

    GisUtil_GridLayerMaximum = lugrid.Maximum
  End Function

  Public Function GisUtil_NumFeaturesInLayer(ByVal layerindex As Long) As Long
    Dim lLayer As MapWindow.Interfaces.Layer

    lLayer = pMapWin.Layers(layerindex)
    Dim sf As New MapWinGIS.Shapefile
    sf = lLayer.GetObject
    GisUtil_NumFeaturesInLayer = sf.NumShapes
  End Function

  Public Function GisUtil_CellValueNthFeatureInLayer(ByVal layerindex As Long, ByVal fieldindex As Long, ByVal featureindex As Long) As Object
    Dim lLayer As MapWindow.Interfaces.Layer

    lLayer = pMapWin.Layers(layerindex)
    Dim sf As New MapWinGIS.Shapefile
    sf = lLayer.GetObject
    GisUtil_CellValueNthFeatureInLayer = sf.CellValue(fieldindex, featureindex)
  End Function

  Public Function GisUtil_AreaNthFeatureInLayer(ByVal layerindex As Long, ByVal featureindex As Long) As Double
    Dim lLayer As MapWindow.Interfaces.Layer

    lLayer = pMapWin.Layers(layerindex)
    Dim sf As New MapWinGIS.Shapefile
    sf = lLayer.GetObject
    Dim sfshape As New MapWinGIS.Shape
    sfshape = sf.Shape(featureindex)
    Dim utilArea As New MapWinGIS.Utils
    GisUtil_AreaNthFeatureInLayer = utilArea.Area(sfshape)
  End Function

  Public Function GisUtil_LengthNthFeatureInLayer(ByVal layerindex As Long, ByVal featureindex As Long) As Double
    Dim lLayer As MapWindow.Interfaces.Layer

    lLayer = pMapWin.Layers(layerindex)
    Dim sf As New MapWinGIS.Shapefile
    sf = lLayer.GetObject
    Dim sfshape As New MapWinGIS.Shape
    sfshape = sf.Shape(featureindex)
    Dim utilLength As New MapWinGIS.Utils
    GisUtil_LengthNthFeatureInLayer = utilLength.Length(sfshape)
  End Function

  Public Sub GisUtil_EndPointsOfLine(ByVal layerindex As Long, ByVal featureindex As Long, ByRef x1 As Double, ByRef y1 As Double, ByRef x2 As Double, ByRef y2 As Double)
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

  Public Sub GisUtil_PointXY(ByVal layerindex As Long, ByVal featureindex As Long, ByRef x As Double, ByRef y As Double)
    Dim lLayer As MapWindow.Interfaces.Layer

    lLayer = pMapWin.Layers(layerindex)
    Dim sf As New MapWinGIS.Shapefile
    sf = lLayer.GetObject
    Dim sfshape As New MapWinGIS.Shape
    sfshape = sf.Shape(featureindex)
    x = sfshape.Point(0).x
    y = sfshape.Point(0).y
  End Sub

  Public Function GisUtil_RemoveFeatureFromLayer(ByVal layerindex As Long, ByVal featureindex As Long) As Double
    Dim lLayer As MapWindow.Interfaces.Layer
    Dim bsuc As Boolean

    lLayer = pMapWin.Layers(layerindex)
    Dim sf As New MapWinGIS.Shapefile
    sf = lLayer.GetObject
    sf.StartEditingShapes(True)
    bsuc = sf.EditDeleteShape(featureindex)
    sf.StopEditingShapes(True, True)
  End Function

  Public Sub GisUtil_SetValueNthFeatureInLayer(ByVal layerindex As Long, ByVal fieldindex As Long, ByVal featureindex As Long, ByVal value As Object)
    Dim lLayer As MapWindow.Interfaces.Layer
    Dim bsuc As Boolean

    lLayer = pMapWin.Layers(layerindex)
    Dim sf As MapWinGIS.Shapefile
    sf = lLayer.GetObject

    bsuc = sf.StartEditingTable()
    bsuc = sf.EditCellValue(fieldindex, featureindex, value)
    bsuc = sf.StopEditingTable()
  End Sub

  Public Function GisUtil_NumSelectedFeaturesInLayer(ByVal layerindex As Long) As Long
    Dim lLayer As MapWindow.Interfaces.Layer
    Dim lsi As MapWindow.Interfaces.SelectInfo

    lsi = pMapWin.View.SelectedShapes
    lLayer = pMapWin.Layers(layerindex)
    If lsi.LayerHandle = layerindex Then
      GisUtil_NumSelectedFeaturesInLayer = lsi.NumSelected()
    Else
      GisUtil_NumSelectedFeaturesInLayer = 0
    End If
  End Function

  Public Function GisUtil_IndexOfNthSelectedFeatureInLayer(ByVal nth As Long, ByVal layerindex As Long) As Long
    Dim lLayer As MapWindow.Interfaces.Layer
    Dim lsi As MapWindow.Interfaces.SelectInfo

    lsi = pMapWin.View.SelectedShapes
    lLayer = pMapWin.Layers(layerindex)
    If lsi.LayerHandle = layerindex Then
      GisUtil_IndexOfNthSelectedFeatureInLayer = lsi.Item(nth).ShapeIndex
    End If
  End Function

  'Public Sub GisUtil_ExtentsOfLayer(ByVal layerindex As Long, ByVal xmax As Double, ByVal xmin As Double, ByVal ymax As Double, ByVal ymin As Double)
  '  Dim lLayer As MapWindow.Interfaces.Layer

  '  lLayer = pMapWin.Layers(layerindex)
  '  Dim sf As New MapWinGIS.Shapefile
  '  sf = lLayer.GetObject
  '  xmax = sf.Extents.xMax
  '  xmin = sf.Extents.xMin
  '  ymax = sf.Extents.yMax
  '  ymin = sf.Extents.yMin
  'End Sub

  'Public Function GisUtil_CreateNewShapefile(ByVal basename As String, ByVal type As Long) As Boolean
  '  Dim osf As New MapWinGIS.Shapefile
  '  'MapWinGIS.ShpfileType.SHP_POLYGON = 5
  '  osf.CreateNew(basename, type)
  'End Function

  Public Function GisUtil_ProjectFileName() As String
    GisUtil_ProjectFileName = pMapWin.Project.FileName
  End Function

  Public Function GisUtil_PointInPolygon(ByVal PointLayerIndex As Long, ByVal nthPoint As Long, ByVal PolygonLayerIndex As Long) As Long
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
    GisUtil_PointInPolygon = polygonsf.PointInShapefile(x, y)
    polygonsf.EndPointInShapefile()
  End Function

  Public Function GisUtil_PointInPolygonXY(ByVal x As Double, ByVal y As Double, ByVal PolygonLayerIndex As Long) As Long
    'given a point xy and a polygon layer, return the polygon this point is in
    Dim lLayer As MapWindow.Interfaces.Layer
    Dim polygonsf As New MapWinGIS.Shapefile

    lLayer = pMapWin.Layers(PolygonLayerIndex)
    polygonsf = lLayer.GetObject

    polygonsf.BeginPointInShapefile()
    GisUtil_PointInPolygonXY = polygonsf.PointInShapefile(x, y)
    polygonsf.EndPointInShapefile()
  End Function

  Public Function GisUtil_AddLayerToMap(ByVal newfilename As String, ByVal layername As String) As Boolean
    'given a shape file name, add it to the map.
    'return true if the layer is already there or successfully added.
    Dim lLayer As MapWindow.Interfaces.Layer

    If GisUtil_FindLayerIndexByName(layername) > -1 Then
      'already on map 
      GisUtil_AddLayerToMap = True
    Else
      lLayer = pMapWin.Layers.Add(newfilename, layername)
      If lLayer Is Nothing Then
        GisUtil_AddLayerToMap = False
      Else
        lLayer.Visible = False
        GisUtil_AddLayerToMap = True
      End If
    End If
  End Function

  Public Function GisUtil_SetLayerVisible(ByVal layerindex As Integer, ByVal vis As Boolean)
    'given a shape file name, add it to the map.
    'return true if the layer is already there or successfully added.
    pMapWin.Layers(layerindex).Visible = vis
  End Function

  Public Function GisUtil_RemoveLayerFromMap(ByVal layerindex As Long) As Boolean
    'given a layer index, remove it from the map.
    pMapWin.Layers.Remove(layerindex)
  End Function

  Public Sub GisUtil_TabulateAreas(ByVal gridLayerIndex As Long, ByVal polygonLayerIndex As Long, _
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
    Dim ic As Long
    Dim ir As Long
    Dim xpos As Double
    Dim ypos As Double
    Dim subid As Integer
    Dim luid As Integer
    Dim lcellarea As Double
    'Dim totalcellcount As Long
    'Dim cellcount As Long
    'Dim lastdisplayed As Long
    Dim startingcolumn As Long
    Dim endingcolumn As Long
    Dim startingrow As Long
    Dim endingrow As Long
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

  Public Sub GisUtil_GridMinMaxInPolygon(ByVal gridLayerIndex As Long, ByVal polygonLayerIndex As Long, _
    ByVal polygonfeatureindex As Long, ByVal Min As Double, ByVal Max As Double)
    'Given a grid and a polygon layer, find the min and max grid value within the feature.

    Dim ic As Long
    Dim ir As Long
    Dim xpos As Double
    Dim ypos As Double
    Dim subid As Integer
    Dim startingcolumn As Long
    Dim endingcolumn As Long
    Dim startingrow As Long
    Dim endingrow As Long
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

  Public Function GisUtil_GridValueAtPoint(ByVal GridLayerIndex As Integer, ByVal x As Double, ByVal y As Double) As Integer
    Dim column As Long
    Dim row As Long
    Dim endingrow As Long
    Dim gridLayer As MapWindow.Interfaces.Layer

    'set input grid
    gridLayer = pMapWin.Layers(GridLayerIndex)
    Dim InputGrid As New MapWinGIS.Grid
    InputGrid = gridLayer.GetGridObject

    InputGrid.ProjToCell(x, y, column, row)
    GisUtil_GridValueAtPoint = InputGrid.Value(column, row)
  End Function

  Public Sub GisUtil_Overlay(ByVal Layer1Name As String, ByVal Layer1FieldName As String, _
                             ByVal Layer2Name As String, ByVal Layer2FieldName As String, _
                             ByVal OutputLayerName As String, ByVal CreateNew As Boolean)
    'overlay layer1 and layer2 (eg landuse and subbasins), creating a polygon layer 
    'containing features from both layers

    Dim i As Long
    Dim k As Long
    Dim shapeindex As Long
    Dim Layer1Index As Long
    Dim Layer1FieldIndex As Long
    Dim Layer2Index As Long
    Dim Layer2FieldIndex As Long
    Dim totalpolygoncount As Long
    Dim polygoncount As Long
    Dim lastdisplayed As Long
    Dim SSFext As MapWinGIS.Extents
    Dim LUext As MapWinGIS.Extents
    Dim area As Double
    Dim Feature1Id As String
    Dim Feature2Id As String
    Dim bsuc As Boolean

    'set layer 1 (landuse)
    Layer1Index = GisUtil_FindLayerIndexByName(Layer1Name)
    Layer1FieldIndex = GisUtil_FindFieldIndexByName(Layer1Index, Layer1FieldName)
    Dim lLayer As MapWindow.Interfaces.Layer
    lLayer = pMapWin.Layers(Layer1Index)
    Dim lusf As New MapWinGIS.Shapefile
    lusf = lLayer.GetObject

    'set layer 2 (subbasins)
    Layer2Index = GisUtil_FindLayerIndexByName(Layer2Name)
    Layer2FieldIndex = GisUtil_FindFieldIndexByName(Layer2Index, Layer2FieldName)
    Dim sLayer As MapWindow.Interfaces.Layer
    sLayer = pMapWin.Layers(Layer2Index)
    Dim ssf As New MapWinGIS.Shapefile
    ssf = sLayer.GetObject
    SSFext = ssf.Extents

    'if any of layer 2 is selected, use only those
    Dim cSelectedSubbasins As New Collection
    For i = 1 To GisUtil_NumSelectedFeaturesInLayer(Layer2Index)
      cSelectedSubbasins.Add(GisUtil_IndexOfNthSelectedFeatureInLayer(i - 1, Layer2Index))
    Next
    If cSelectedSubbasins.Count = 0 Then
      'no subbasins selected, act as if all are selected
      For i = 1 To GisUtil_NumFeaturesInLayer(Layer2Index)
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

  Public Function GisUtil_ClipShapesWithPolygon(ByVal inputlayerindex As Integer, ByVal clipperlayerindex As Integer) As String
    'returns output shape file name as string 
    Dim mx As MapWinX.SpatialOperations
    Dim i As Integer
    Dim j As Integer
    Dim k As Integer
    Dim bsuc As Boolean
    Dim newname As String

    GisUtil_ClipShapesWithPolygon = ""
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
    clipperpath = PathNameOnly(GisUtil_LayerFileName(clipperlayerindex))

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
        If GisUtil_IsThisShapeTheSameAsOrPartOfAnotherShape(orsf.Shape(i - 1), isf.Shape(j - 1)) Then
          'found matching shape, copy attributes
          For k = 1 To isf.NumFields
            bsuc = orsf.EditCellValue(k - 1, i - 1, isf.CellValue(k - 1, j - 1))
          Next k
        End If
      Next j
    Next i

    bsuc = orsf.SaveAs(newname)
    GisUtil_ClipShapesWithPolygon = newname

  End Function

  Private Function GisUtil_IsThisShapeTheSameAsOrPartOfAnotherShape(ByVal sf1shape As MapWinGIS.Shape, ByVal sf2shape As MapWinGIS.Shape) As Boolean
    'given one shape and another which may be a piece of the first shape,
    'determine if the second shape is equivalent or part of the first shape
    '(after a reach segment has been clipped to a polygon boundary, we still need 
    'to identify which original reach it was a part of)
    Dim i As Integer
    Dim j As Integer
    Dim foundpoint As Boolean

    GisUtil_IsThisShapeTheSameAsOrPartOfAnotherShape = True

    For j = 2 To sf1shape.numPoints - 1
      foundpoint = False
      For i = 1 To sf2shape.numPoints
        If sf1shape.Point(j - 1).x = sf2shape.Point(i - 1).x And sf1shape.Point(j - 1).y = sf2shape.Point(i - 1).y Then
          foundpoint = True
          Exit For
        End If
      Next i
      If foundpoint = False Then
        GisUtil_IsThisShapeTheSameAsOrPartOfAnotherShape = False
        Exit Function
      End If
    Next j

  End Function

  Public Sub GisUtil_MergeFeaturesBasedOnAttribute(ByVal LayerIndex, ByVal FieldIndex)
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
      targetval = GisUtil_CellValueNthFeatureInLayer(LayerIndex, FieldIndex, i)
      endx = shape1.Point(shape1.numPoints - 1).x
      endy = shape1.Point(shape1.numPoints - 1).y
      j = 0
      Do While j < isf.NumShapes
        If i <> j Then
          shape2 = isf.Shape(j)
          thisval = GisUtil_CellValueNthFeatureInLayer(LayerIndex, FieldIndex, j)
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
      targetval = GisUtil_CellValueNthFeatureInLayer(LayerIndex, FieldIndex, i)
      startx1 = shape1.Point(0).x
      starty1 = shape1.Point(0).y
      endx1 = shape1.Point(shape1.numPoints - 1).x
      endy1 = shape1.Point(shape1.numPoints - 1).y
      j = 0
      Do While j < isf.NumShapes
        If i <> j Then
          shape2 = isf.Shape(j)
          thisval = GisUtil_CellValueNthFeatureInLayer(LayerIndex, FieldIndex, j)
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

End Module
