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
    Dim lLayer2 As MapWindow.Interfaces.Layer
    Dim sf2 As New MapWinGIS.Shapefile

    lLayer1 = pMapWin.Layers(layerindex1)
    sf1 = lLayer1.GetObject
    lLayer2 = pMapWin.Layers(layerindex2)
    sf2 = lLayer2.GetObject

    GisUtil_OverlappingPolygons = False
    'check if one inside the other
    For k = 1 To sf1.Shape(nthfeature1).numPoints
      x = sf1.Shape(nthfeature1).Point(k - 1).x
      y = sf1.Shape(nthfeature1).Point(k - 1).y
      GisUtil_OverlappingPolygons = sf2.PointInShape(nthfeature2, x, y)
      If GisUtil_OverlappingPolygons Then
        'quit if we've found they overlap
        Exit For
      End If
    Next k
    If Not GisUtil_OverlappingPolygons Then
      'now check the opposite
      For k = 1 To sf2.Shape(nthfeature2).numPoints
        x = sf2.Shape(nthfeature2).Point(k - 1).x
        y = sf2.Shape(nthfeature2).Point(k - 1).y
        GisUtil_OverlappingPolygons = sf1.PointInShape(nthfeature1, x, y)
        If GisUtil_OverlappingPolygons Then
          'quit if we've found they overlap
          Exit For
        End If
      Next k
    End If
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
    polygonsf = lLayer.GetObject

    polygonsf.BeginPointInShapefile()
    x = pointsf.Shape(nthPoint - 1).Point(0).x
    y = pointsf.Shape(nthPoint - 1).Point(0).y
    GisUtil_PointInPolygon = polygonsf.PointInShapefile(x, y)
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
      cSelectedSubbasins.Add(GisUtil_IndexOfNthSelectedFeatureInLayer(i, Layer2Index))
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
    totalpolygoncount = lusf.NumShapes * cSelectedSubbasins.Count
    polygoncount = 0
    lastdisplayed = 0
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
      End If
    Next i

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

End Module
