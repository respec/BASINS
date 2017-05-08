Attribute VB_Name = "ShapeDefines"
Option Explicit
'*****************************************************************************************
'ShapeDefines
'
'Contains all of the user defined types for the valid shape file types


'*****************************************************************************************
'Main file header stucture for both the shape and index files
Public Type T_MainFileHeader
    FileCode As Long        'big
    u1 As Long              'big
    u2 As Long              'big
    u3 As Long              'big
    u4 As Long              'big
    u5 As Long              'big
    FileLength As Long      'big
    version As Long         'Little
    ShapeType As Long       'Little
    BndBoxXmin As Double    'Little
    BndBoxYmin As Double    'Little
    BndBoxXmax As Double    'Little
    BndBoxYmax As Double    'Little
    BndBoxZmin As Double    'Little
    BndBoxZmax As Double    'Little
    BndBoxMmin As Double    'Little
    BndBoxMmax As Double    'Little
End Type

'*****************************************************************************************
'Shape file record header structure
Public Type T_RecordHeader
    RecordNumber As Long    'big
    ContentLength As Long   'big
End Type

'*****************************************************************************************
'index file record structure
Public Type T_IndexRecordHeader
    offset As Long              'big
    ContentLength As Long       'big
End Type

'*****************************************************************************************
'Structure for the null shape
Public Type T_NullShape
    ShapeType As Long
End Type

'*****************************************************************************************
' Basic point structure for standard point line and polygon types
Public Type T_BasicPoint
    x As Double
    y As Double
End Type
'*****************************************************************************************
'structure for the bounding box
Public Type T_BoundingBox
    xMin As Double
    yMin As Double
    xMax As Double
    yMax As Double
End Type

'*****************************************************************************************
'structure for the XYPoint shapetype
Public Type T_shpXYPoint
    ShapeType As Long
    thePoint As T_BasicPoint
End Type

'*****************************************************************************************
'structure for the MultiPoint, polyline, polygon, MultiPatch shape types
Public Type T_shpPoly
    ShapeType As Long
    Box As T_BoundingBox
    NumParts As Long    'Always 0 for XYMultiPoint
    NumPoints As Long
    Parts() As Long     'Always empty for MultiPoint types
    PartTypes() As Long 'Only used by MultiPatch
    thePoints() As T_BasicPoint
    'Z and M not used by simple PolyLine, Polygon, MultiPoint
    Zmin As Double
    Zmax As Double
    Zarray() As Double
    Mmin As Double
    Mmax As Double
    Marray() As Double
End Type

'*****************************************************************************************
'structure for the pointM and pointZ types
Public Type T_shpPointMZ
    ShapeType As Long
    thePoint As T_BasicPoint
    Z As Double
    M As Double
End Type

Public Function PointInPolygon(x As Double, y As Double, poly As T_shpPoly) As Boolean
  With poly
    'First, it has to be within bounding box.
    If x >= .Box.xMin And x <= .Box.xMax And y >= .Box.yMin And y <= .Box.yMax Then
      Dim iPart As Long
      Dim iPoint As Long
      Dim iMaxPoint As Long
      Dim pt1 As T_BasicPoint
      Dim pt2 As T_BasicPoint
      Dim inside As Boolean
      
      iPoint = 0
      For iPart = 0 To .NumParts - 1
        If iPart < .NumParts - 1 Then
          iMaxPoint = .Parts(iPart + 1) - 1
        Else
          iMaxPoint = .NumPoints - 1
        End If
        pt1 = .thePoints(iPoint)
        iPoint = iPoint + 1
        While iPoint <= iMaxPoint
          pt2 = .thePoints(iPoint)
          If pt1.y > y Then
            If pt2.y > y Then GoTo NextSide 'side does not intersect horizontal ray - y-values do not overlap
          Else
            If pt2.y < y Then GoTo NextSide 'side does not intersect horizontal ray - y-values do not overlap
          End If
          If pt1.x < x Then
            If pt2.x < x Then GoTo NextSide 'side does not intersect +x ray, is entirely on -x side
          Else
            If pt2.x > x Then
              PointInPolygon = Not PointInPolygon
              GoTo NextSide 'side intersects: two y values are on different sides, both x on +x side
            End If
          End If
          'Have to do angle test because crossing is not clear from simple tests
          If ((pt2.x - (pt2.y - y) * (pt1.x - pt2.x) / (pt1.y - pt2.y)) >= x) Then
            PointInPolygon = Not PointInPolygon
          End If
NextSide:
          pt1 = pt2
          iPoint = iPoint + 1
        Wend
        'commented out to keep flipping PointInPolygon in other parts -- a polygon might have a "hole"
        'If PointInPolygon Then Exit Function
      Next
    Else 'Not in bounding box, so not within poly
      PointInPolygon = False
    End If
  End With
End Function

'True if any segment of poly intersects the segment (X1, Y1)-(X2, Y2)
Public Function LineIntersectsPolygon(X1 As Double, Y1 As Double, _
                                      X2 As Double, Y2 As Double, _
                                      poly As T_shpPoly) As Boolean
  With poly
    Dim iPart As Long
    Dim iPoint As Long
    Dim iMaxPoint As Long
    Dim pt1 As T_BasicPoint
    Dim pt2 As T_BasicPoint
    Dim inside As Boolean
    Dim dxLine As Double
    Dim dyLine As Double
    Dim dxPoly As Double
    Dim dyPoly As Double
    Dim denom As Double
    Dim dy1 As Double
    Dim dx1 As Double
    Dim s As Double
    Dim t As Double
    
    dxLine = X2 - X1
    dyLine = Y2 - Y1

    iPoint = 0
    For iPart = 0 To .NumParts - 1
      pt1 = .thePoints(iPoint)
      If iPart < .NumParts - 1 Then
        iMaxPoint = .Parts(iPart + 1)
      Else
        iMaxPoint = .NumPoints
      End If
      iPoint = iPoint + 1
      While iPoint <= iMaxPoint - 1
        pt2 = .thePoints(iPoint)
        
        dxPoly = pt2.x - pt1.x
        dyPoly = pt2.y - pt1.y
        denom = dxPoly * dyLine - dyPoly * dxLine
        
        If denom = 0 Then 'Line Parallel to edge
          'TODO: decide whether intersection along a line can count
          'If so, determine whether they overlap
        Else
          dy1 = pt1.y - Y1
          dx1 = X1 - pt1.x
          s = (dxLine * dy1 + dyLine * dx1) / denom
          If s >= 0# And s <= 1# Then
            t = (dxPoly * dy1 + dyPoly * dx1) / denom
            If t >= 0# And t <= 1# Then
              LineIntersectsPolygon = True
              Exit Function
            End If
          End If
        End If
        pt1 = pt2
        iPoint = iPoint + 1
      Wend
    Next
  End With
End Function

Public Function AnyPointInPolygon(aPolyPoints As T_shpPoly, aPolygon As T_shpPoly) As Boolean
  Dim iPart As Long
  Dim iPoint As Long
  Dim iMaxPoint As Long
  Dim pt As T_BasicPoint
  
  iPoint = 0
  iMaxPoint = aPolyPoints.NumPoints - 1
  While iPoint <= iMaxPoint
    pt = aPolyPoints.thePoints(iPoint)
    If PointInPolygon(pt.x, pt.y, aPolygon) Then
      AnyPointInPolygon = True
      Exit Function
    End If
    iPoint = iPoint + 1
  Wend
  AnyPointInPolygon = False
End Function

Private Function AnyLineIntersectsPolygon(aPolyLine As T_shpPoly, aPolygon As T_shpPoly) As Boolean
  Dim iPart As Long
  Dim iPoint As Long
  Dim iMaxPoint As Long
  Dim pt1 As T_BasicPoint
  Dim pt2 As T_BasicPoint
  
  AnyLineIntersectsPolygon = False
  
  iPoint = 0
  iMaxPoint = aPolyLine.NumPoints - 1
  While iPoint < iMaxPoint
    pt1 = aPolyLine.thePoints(iPoint)
    iPoint = iPoint + 1
    pt2 = aPolyLine.thePoints(iPoint)
    iPoint = iPoint + 1
    If LineIntersectsPolygon(pt1.x, pt1.y, pt2.x, pt2.y, aPolygon) Then
      AnyLineIntersectsPolygon = True
      Exit Function
    End If
  Wend
  'For polygon types, check implied edge back to first point
  Select Case aPolyLine.ShapeType
    Case FILETYPEENUM.typePolygon, _
         FILETYPEENUM.typePolygonZ, _
         FILETYPEENUM.typePolygonM:
      pt1 = aPolyLine.thePoints(0)
      If LineIntersectsPolygon(pt1.x, pt1.y, pt2.x, pt2.y, aPolygon) Then
        AnyLineIntersectsPolygon = True
      End If
  End Select
End Function

Public Function PolyLineOverlapsPolygon(aPolyLine As T_shpPoly, aPolygon As T_shpPoly) As Boolean
  PolyLineOverlapsPolygon = False
  'First, if bounding boxes do not overlap, then polygons do not overlap.
  If aPolyLine.Box.xMax < aPolygon.Box.xMin Then
  ElseIf aPolyLine.Box.yMax < aPolygon.Box.yMin Then
  ElseIf aPolyLine.Box.xMin > aPolygon.Box.xMax Then
  ElseIf aPolyLine.Box.yMin > aPolygon.Box.yMax Then
  
  ElseIf AnyPointInPolygon(aPolyLine, aPolygon) Then
    PolyLineOverlapsPolygon = True
'  Skipping this test is the only difference from PolygonsOverlap
'  ElseIf AnyPointInPolygon(aPoly2, aPoly1) Then
'    PolygonsOverlap = True
  
  ElseIf AnyLineIntersectsPolygon(aPolyLine, aPolygon) Then
    PolyLineOverlapsPolygon = True
  End If
End Function

'True if any point of either polygon is in the other or if any edges intersect
Function PolygonsOverlap(aPoly1 As T_shpPoly, aPoly2 As T_shpPoly) As Boolean
  PolygonsOverlap = False
  'First, if bounding boxes do not overlap, then polygons do not overlap.
  If aPoly1.Box.xMax < aPoly2.Box.xMin Then
  ElseIf aPoly1.Box.yMax < aPoly2.Box.yMin Then
  ElseIf aPoly1.Box.xMin > aPoly2.Box.xMax Then
  ElseIf aPoly1.Box.yMin > aPoly2.Box.yMax Then
  
  ElseIf AnyPointInPolygon(aPoly1, aPoly2) Then
    PolygonsOverlap = True
  ElseIf AnyPointInPolygon(aPoly2, aPoly1) Then
    PolygonsOverlap = True
  
  ElseIf AnyLineIntersectsPolygon(aPoly1, aPoly2) Then
    PolygonsOverlap = True
  End If
End Function

'aDBF As clsDBF
'aLatitudeYfield  - field number in aDBF containing Latitude or "Y" values
'aLongitudeXfield - field number in aDBF containing Longitude or "X" values
Public Sub WriteShapePointsFromDBF(aDBF As Object, aLatitudeYfield As Long, aLongitudeXfield As Long)
  Dim shp As CShape_IO
  Dim XYPoint As T_shpXYPoint
  Dim iRecord As Long
  Dim vCoordinate As Variant
  Dim shpFilename As String
  Dim nNonNumericLat As Long
  Dim nNonNumericLon As Long
  
  XYPoint.ShapeType = FILETYPEENUM.typePoint 'typePoint = 1
  Set shp = New CShape_IO
  shpFilename = FilenameNoExt(aDBF.filename) & ".shp"
  shp.CreateNewShape shpFilename, XYPoint.ShapeType
  For iRecord = 1 To aDBF.NumRecords
    aDBF.CurrentRecord = iRecord
    vCoordinate = aDBF.Value(aLongitudeXfield)
    If IsNumeric(vCoordinate) Then
      XYPoint.thePoint.x = vCoordinate
    Else
      XYPoint.thePoint.x = 0
      nNonNumericLon = nNonNumericLon + 1
      AppendFileString FilenameNoExt(shpFilename) & ".err", "Non-numeric longitude at " & iRecord & ": " & vCoordinate & vbCrLf
    End If
    
    vCoordinate = aDBF.Value(aLatitudeYfield)
    If IsNumeric(vCoordinate) Then
      XYPoint.thePoint.y = vCoordinate
    Else
      XYPoint.thePoint.y = 0
      nNonNumericLat = nNonNumericLat + 1
      AppendFileString FilenameNoExt(shpFilename) & ".err", "Non-numeric latitude at " & iRecord & ": " & vCoordinate & vbCrLf
    End If
    shp.putXYPoint 0, XYPoint
  Next
  shp.FileShutDown
  If nNonNumericLon > 0 Then Debug.Print nNonNumericLon & " non-numeric longitudes in " & shpFilename
  If nNonNumericLat > 0 Then Debug.Print nNonNumericLat & " non-numeric latitudes in " & shpFilename
End Sub


