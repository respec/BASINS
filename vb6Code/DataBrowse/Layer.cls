VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "Layer"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Attribute VB_Description = "Map Layer"
Attribute VB_Ext_KEY = "SavedWithClassBuilder" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"
Option Explicit

Private Const maxCacheableFileSize = 10000      'bytes in largest shape file to cache
Private Const border = 0.05 'percent of area to add as border around stated bounds of data in layer

Private pPath As String
Private pBaseFilename As String 'ESRI shape file this layer will be drawn from

Private header As ShapeDefines.T_MainFileHeader
Private record&                                 'current record while drawing
Private nRecords&                               'number of records in layer
Private nRecordsCached&                         'number of records stored locally in array
Private XYPoints() As ShapeDefines.T_shpXYPoint
Private XYMultiPoints() As ShapeDefines.T_shpXYMultiPoint
Private polylines() As ShapeDefines.T_shpPolyLine
Private polygons() As ShapeDefines.T_shpPolygon

'Sets picturebox scale to fit at least the given bounding box
'Resulting scale will probably be larger in one dimension to preserve aspect ratio
Public Sub SetScale(P As PictureBox, xMin#, xMax#, yMin#, yMax#)
  Dim scaleWidth#, scaleHeight#, xRatio#, yRatio#, aspectRatio#, Top#, Left#
  
  scaleWidth = xMax - xMin
  scaleHeight = yMin - yMax
  xRatio = scaleWidth / P.Width
  yRatio = scaleHeight / P.Height
  If Abs(xRatio) > Abs(yRatio) Then
    aspectRatio = Abs(xRatio) * 1.05
  Else
    aspectRatio = Abs(yRatio) * 1.05
  End If
    
  If scaleWidth < 0 Then
    scaleWidth = -aspectRatio * P.Width
  Else
    scaleWidth = aspectRatio * P.Width
  End If
  
  If scaleHeight < 0 Then
    scaleHeight = -aspectRatio * P.Height
  Else
    scaleHeight = aspectRatio * P.Height
  End If
  
  P.ScaleLeft = (xMin + xMax) / 2 - scaleWidth / 2
  P.ScaleTop = (yMin + yMax) / 2 - scaleHeight / 2
  On Error Resume Next
  P.scaleWidth = scaleWidth
  P.scaleHeight = scaleHeight
End Sub

'Sets picturebox scale to fit this layer, preserving aspect ratio
'if expand=true, will not shrink scale, only expands if necessary to display all data in this layer
Public Sub SetMaxScale(P As PictureBox, expand As Boolean)
  Dim xMin#, xMax#, yMin#, yMax#
  If expand Then
    xMin = P.ScaleLeft
    xMax = P.ScaleLeft + P.scaleWidth
    yMax = P.ScaleTop
    yMin = P.ScaleTop + P.scaleHeight
    If P.scaleWidth > 0 Then
      If header.BndBoxXmin < xMin Then xMin = header.BndBoxXmin
      If header.BndBoxXmax > xMax Then xMax = header.BndBoxXmax
    Else
      If header.BndBoxXmin > xMin Then xMin = header.BndBoxXmin
      If header.BndBoxXmax < xMax Then xMax = header.BndBoxXmax
    End If
    If P.scaleHeight > 0 Then
      If header.BndBoxYmin < yMin Then yMin = header.BndBoxYmin
      If header.BndBoxYmax > yMax Then yMax = header.BndBoxYmax
    Else
      If header.BndBoxYmin > yMin Then yMin = header.BndBoxYmin
      If header.BndBoxYmax < yMax Then yMax = header.BndBoxYmax
    End If
  Else
    xMin = header.BndBoxXmin
    xMax = header.BndBoxXmax
    yMin = header.BndBoxYmin
    yMax = header.BndBoxYmax
  End If
  
  SetScale P, xMin, xMax, yMin, yMax

End Sub

Public Sub Render(P As PictureBox)
  Dim shapeIO As New CShape_IO
  record = 1
  Select Case header.ShapeType
    Case 0 'Null
    Case 1 'Points
      P.ForeColor = vbBlack
      If nRecordsCached > 0 Then RenderPointsFromCache P
      If nRecordsCached < nRecords Then RenderPointsFromFile P
    Case 3 'PolyLine
      P.ForeColor = vbBlue
      If nRecordsCached > 0 Then RenderPolylinesFromCache P
      If nRecordsCached < nRecords Then RenderPolylinesFromFile P
    Case 5 'Polygon
      P.ForeColor = vbBlack
      If nRecordsCached > 0 Then RenderPolygonsFromCache P
      If nRecordsCached < nRecords Then RenderPolygonsFromFile P
    Case 8 'MultiPoint
      If nRecordsCached > 0 Then RenderMultiPointsFromCache P
      If nRecordsCached < nRecords Then RenderMultiPointsFromFile P
  End Select
End Sub

Public Property Get ShapeFile$()
  ShapeFile = pPath & pBaseFilename & ".shp"
End Property

'Set the ESRI shape file this layer will be drawn from
'Header information is read for later use
Public Property Let ShapeFile(ByVal newValue$)
  Dim shapeIO As New CShape_IO
  On Error GoTo NeverMind
  pPath = PathNameOnly(newValue)
  pBaseFilename = FilenameOnly(newValue)
  If Len(pBaseFilename) > 3 Then
    If Mid(pBaseFilename, Len(pBaseFilename) - 3, 1) = "." Then
      pBaseFilename = Left(pBaseFilename, Len(pBaseFilename) - 4)
    End If
  End If
  
  shapeIO.ShapeFileOpen pPath & "\" & pBaseFilename & ".shp", vbReadOnly
  nRecords = shapeIO.getRecordCount
  nRecordsCached = 0
  header = shapeIO.getShapeHeader
  If FileLen(pPath & "\" & pBaseFilename & ".shp") < maxCacheableFileSize And nRecords > 0 Then
    ReadIn shapeIO
  End If
  shapeIO.FileShutDown
  Exit Property
NeverMind:
  pPath = ""
  pBaseFilename = ""
  nRecords = 0
End Property

'Read drawing information into memory so drawing will be faster
Private Sub ReadIn(shapeIO As CShape_IO)
  nRecordsCached = nRecords
  Select Case header.ShapeType
    Case 0 'Null
    Case 1 'Points
      ReDim XYPoints(1 To nRecords)
      For record = 1 To nRecords
        XYPoints(record) = shapeIO.getXYPoint(record)
      Next record
    Case 3 'PolyLine
      ReDim polylines(1 To nRecords)
      For record = 1 To nRecords
        polylines(record) = shapeIO.getPolyLine(record)
      Next record
    Case 5 'Polygon
      ReDim polygons(1 To nRecords)
      For record = 1 To nRecords
        polygons(record) = shapeIO.getPolygon(record)
      Next record
    Case 8 'MultiPoint
      ReDim XYMultiPoints(1 To nRecords)
      For record = 1 To nRecords
        XYMultiPoints(record) = shapeIO.getXYMultiPoint(record)
      Next record
    Case Else: 'type not implemented, none read in
      nRecordsCached = 0
  End Select
End Sub

Private Sub RenderPointsFromFile(P As PictureBox)
  Dim shapeIO As New CShape_IO
  Dim aXYPoint As ShapeDefines.T_shpXYPoint
  Dim radius!
  
  shapeIO.ShapeFileOpen pPath & "\" & pBaseFilename & ".shp", vbReadOnly
  radius = P.scaleWidth / 100
  While record <= nRecords
    aXYPoint = shapeIO.getXYPoint(record)
    P.Circle (aXYPoint.thePoint.x, aXYPoint.thePoint.y), radius
    record = record + 1
  Wend
  shapeIO.FileShutDown
End Sub

Private Sub RenderPointsFromCache(P As PictureBox)
  Dim radius!
  On Error Resume Next
  radius = P.scaleWidth / 100
  While record <= nRecordsCached
    'Debug.Print XYPoints(record).thePoint.x, XYPoints(record).thePoint.y
    P.Circle (XYPoints(record).thePoint.x, XYPoints(record).thePoint.y), radius
    record = record + 1
  Wend
End Sub

Private Sub RenderMultiPointsFromFile(P As PictureBox)
  Dim shapeIO As New CShape_IO
  Dim aXYMultiPoint As ShapeDefines.T_shpXYMultiPoint
  Dim radius!
  Dim point&
  
  shapeIO.ShapeFileOpen pPath & "\" & pBaseFilename & ".shp", vbReadOnly
  radius = P.scaleWidth / 100
  While record <= nRecords
    P.ForeColor = QBColor((record - 1) Mod 15)
    If P.ForeColor = P.BackColor Then P.ForeColor = vbBlack
    aXYMultiPoint = shapeIO.getXYMultiPoint(record)
    With aXYMultiPoint
      For point = 0 To .NumPoints - 1
        P.Circle (.thePoints(point).x, .thePoints(point).y), radius
      Next point
    End With
    record = record + 1
  Wend
  shapeIO.FileShutDown
End Sub

Private Sub RenderMultiPointsFromCache(P As PictureBox)
  Dim radius!
  Dim point&
  radius = P.scaleWidth / 100
  While record <= nRecordsCached
    P.ForeColor = QBColor((record - 1) Mod 15)
    If P.ForeColor = P.BackColor Then P.ForeColor = vbBlack
    With XYMultiPoints(record)
      For point = 0 To .NumPoints - 1
        P.Circle (.thePoints(point).x, .thePoints(point).y), radius
      Next point
    End With
    record = record + 1
  Wend
End Sub

Private Sub RenderPolylinesFromFile(P As PictureBox)
  Dim shapeIO As New CShape_IO
  Dim aPolyLine As ShapeDefines.T_shpPolyLine
  Dim part&, point&, maxpoint&
  
  shapeIO.ShapeFileOpen pPath & "\" & pBaseFilename & ".shp", vbReadOnly
  While record <= nRecords
    aPolyLine = shapeIO.getPolyLine(record)
    point = 0
    With aPolyLine
      For part = 0 To .NumParts - 1
        P.ForeColor = QBColor(part Mod 15)
        If P.ForeColor = P.BackColor Then P.ForeColor = vbBlack
        P.CurrentX = .thePoints(point).x
        P.CurrentY = .thePoints(point).y
        If part < .NumParts - 1 Then
          maxpoint = .Parts(part + 1)
        Else
          maxpoint = .NumPoints
        End If
        While point < maxpoint
          P.Line -(.thePoints(point).x, .thePoints(point).y)
          point = point + 1
        Wend
      Next part
    End With
    record = record + 1
  Wend
  shapeIO.FileShutDown
End Sub

Private Sub RenderPolylinesFromCache(P As PictureBox)
  Dim part&, point&, maxpoint&
  While record <= nRecordsCached
    point = 0
    With polylines(record)
      For part = 0 To .NumParts - 1
        P.ForeColor = QBColor(part Mod 15)
        If P.ForeColor = P.BackColor Then P.ForeColor = vbBlack
        P.CurrentX = .thePoints(point).x
        P.CurrentY = .thePoints(point).y
        If part < .NumParts - 1 Then
          maxpoint = .Parts(part + 1)
        Else
          maxpoint = .NumPoints
        End If
        While point < maxpoint
          P.Line -(.thePoints(point).x, .thePoints(point).y)
          point = point + 1
        Wend
      Next part
    End With
    record = record + 1
  Wend
End Sub

Private Sub RenderPolygonsFromFile(P As PictureBox)
  Dim shapeIO As New CShape_IO
  Dim aPolygon As ShapeDefines.T_shpPolygon
  Dim part&, point&, maxpoint&
  
  shapeIO.ShapeFileOpen pPath & "\" & pBaseFilename & ".shp", vbReadOnly
  While record <= nRecords
    aPolygon = shapeIO.getPolygon(record)
    point = 0
    With aPolygon
      For part = 0 To .NumParts - 1
        P.ForeColor = QBColor(part Mod 15)
        If P.ForeColor = P.BackColor Then P.ForeColor = vbBlack
        P.CurrentX = .thePoints(point).x
        P.CurrentY = .thePoints(point).y
        If part < .NumParts - 1 Then
          maxpoint = .Parts(part + 1)
        Else
          maxpoint = .NumPoints
        End If
        While point < maxpoint
          P.Line -(.thePoints(point).x, .thePoints(point).y)
          point = point + 1
        Wend
        'DoEvents
      Next part
    End With
    record = record + 1
  Wend
  shapeIO.FileShutDown
End Sub

Private Sub RenderPolygonsFromCache(P As PictureBox)
  Dim part&, point&, maxpoint&
  
  While record <= nRecordsCached
    point = 0
    With polygons(record)
      For part = 0 To .NumParts - 1
        P.ForeColor = QBColor(part Mod 15)
        If P.ForeColor = P.BackColor Then P.ForeColor = vbBlack
        P.CurrentX = .thePoints(point).x
        P.CurrentY = .thePoints(point).y
        If part < .NumParts - 1 Then
          maxpoint = .Parts(part + 1)
        Else
          maxpoint = .NumPoints
        End If
        While point < maxpoint
          P.Line -(.thePoints(point).x, .thePoints(point).y)
          point = point + 1
        Wend
      Next part
    End With
    record = record + 1
  Wend
End Sub
