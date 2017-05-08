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

Private Const maxCacheableFileSize = 100000000      'bytes in largest shape file to cache
Private Const border = 0.05 'percent of area to add as border around stated bounds of data in layer

Private pPath As String
Private pBaseFilename As String 'ESRI shape file this layer will be drawn from
Private pLabelField As String

Private header As ShapeDefines.T_MainFileHeader
Private Record As Long                          'current record while drawing
Private nRecords As Long                        'number of records in layer
Private nRecordsCached As Long                  'number of records stored locally in array
Private pPoints() As ShapeDefines.T_shpXYPoint
Private pRadius As Single
'Private polylines() As ShapeDefines.T_shpPoly
Private pPolygons() As ShapeDefines.T_shpPoly
Private pColor() As Long
Private pAlwaysFillPolygons As Boolean

'cached values for picture box currently being drawn in
Private pPixelWidth As Long
Private pPixelHeight As Long
Private pHdc As Long
Private pScaleLeft As Single
Private pScaleTop As Single
Private pScaleHeight As Single
Private pScaleWidth As Single

'Private pRendering As Long
'Private pRenderTakeAnumber As Long

'Sets X and Y to coordinates of the center of the shape at Record position
Private Sub ShapeCenter(ByRef X As Single, ByRef Y As Single)
  Select Case header.ShapeType
    Case 0 'Null
      X = 0
      Y = 0
    Case 1 'Points
      X = pPoints(Record).thePoint.X
      Y = pPoints(Record).thePoint.Y
    Case 3, 5, 8 'PolyLine, Polygon, MultiPoint
      With pPolygons(Record).Box
        X = (.xMax + .xMin) / 2
        Y = (.yMax + .yMin) / 2
      End With
    Case Else 'Unsupported shapes
      X = 0
      Y = 0
  End Select
End Sub

Public Property Get ShapeColor(iShape As Long) As Long
  ShapeColor = pColor(iShape)
End Property
Public Property Let ShapeColor(iShape As Long, newValue As Long)
  pColor(iShape) = newValue
End Property

Public Function IndexNearest(X As Single, Y As Single) As Long
  Dim Distance As Single
  Dim DistanceNearestSoFar As Single
  Dim centerX As Single, centerY As Single
  
  DistanceNearestSoFar = 3E+38
  For Record = 1 To nRecordsCached
    ShapeCenter centerX, centerY
    Distance = (X - centerX) ^ 2 + (Y - centerY) ^ 2
    If Distance < DistanceNearestSoFar Then
      DistanceNearestSoFar = Distance
      IndexNearest = Record
    End If
  Next
  'If nRecordsCached < nRecords Then RenderPointsFromFile P
End Function

'Sets picturebox scale to fit at least the given bounding box
'Resulting scale will probably be larger in one dimension to preserve aspect ratio
Public Sub SetScale(P As PictureBox, Left As Double, Right As Double, Top As Double, Bottom As Double)
  Dim scaleWidth As Double, scaleHeight As Double ', scaleLeft As Double, scaleTop As Double
  Dim xRatio As Double, yRatio As Double, aspectRatio As Double
  
  scaleWidth = Right - Left
  scaleHeight = Bottom - Top
  xRatio = Abs(scaleWidth / P.Width)
  yRatio = Abs(scaleHeight / P.Height)
  If xRatio > yRatio Then
    aspectRatio = xRatio * 1.05
  Else
    aspectRatio = yRatio * 1.05
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
  
  On Error Resume Next
  P.scaleLeft = (Left + Right) / 2 - scaleWidth / 2
  P.scaleTop = (Top + Bottom) / 2 - scaleHeight / 2
  P.scaleWidth = scaleWidth
  P.scaleHeight = scaleHeight
End Sub

'Sets picturebox scale to fit this layer, preserving aspect ratio
'if expand=true, will not shrink scale, only expands if necessary to display all data in this layer
Public Sub SetMaxScale(P As PictureBox, expand As Boolean)
  Dim Left As Double, Right As Double, Top As Double, Bottom As Double
  If expand Then
    Left = P.scaleLeft
    Right = P.scaleLeft + P.scaleWidth
    Top = P.scaleTop
    Bottom = P.scaleTop + P.scaleHeight
    If P.scaleWidth > 0 Then
      If header.BndBoxXmin < Left Then Left = header.BndBoxXmin
      If header.BndBoxXmax > Right Then Right = header.BndBoxXmax
    Else
      If header.BndBoxXmin < Right Then Right = header.BndBoxXmin
      If header.BndBoxXmax > Left Then Left = header.BndBoxXmax
    End If
    If P.scaleHeight > 0 Then
      If header.BndBoxYmin < Top Then Top = header.BndBoxYmin
      If header.BndBoxYmax > Bottom Then Bottom = header.BndBoxYmax
    Else
      If header.BndBoxYmin < Bottom Then Bottom = header.BndBoxYmin
      If header.BndBoxYmax > Top Then Top = header.BndBoxYmax
    End If
  Else
    Left = header.BndBoxXmin
    Right = header.BndBoxXmax
    Top = header.BndBoxYmax
    Bottom = header.BndBoxYmin
  End If
  
  SetScale P, Left, Right, Top, Bottom

End Sub

Private Sub GetPixelDimensions(P As PictureBox)
  pHdc = P.hdc
  pScaleLeft = P.scaleLeft
  pScaleTop = P.scaleTop
  pScaleWidth = P.scaleWidth
  pScaleHeight = P.scaleHeight
  P.ScaleMode = vbPixels
  pPixelWidth = P.scaleWidth
  pPixelHeight = P.scaleHeight
  P.Scale (pScaleLeft, pScaleTop)-(pScaleLeft + pScaleWidth, pScaleTop + pScaleHeight)
End Sub

Public Sub Render(P As PictureBox, Optional shapeIndex As Long = 0)
  Dim saveDrawMode As Long
  Dim saveColor As Long
  
  GetPixelDimensions P
  
  pRadius = 30 * P.scaleWidth / pPixelWidth

  saveDrawMode = P.DrawMode
  
  If shapeIndex > 0 Then     'Just rendering one shape
    saveColor = pColor(shapeIndex)
    pAlwaysFillPolygons = True
  ElseIf shapeIndex < 0 Then 'Highlighting a shape
    shapeIndex = -shapeIndex
    saveColor = pColor(shapeIndex)
    pColor(shapeIndex) = vbYellow
    P.DrawMode = vbNotXorPen
    pAlwaysFillPolygons = True
  Else                       'rendering all shapes in layer
    Record = 1
    pAlwaysFillPolygons = False
  End If

  Select Case header.ShapeType
    Case 0 'Null
    Case 1 'Points
      If nRecordsCached > 0 Then RenderPointsFromCache P, shapeIndex
      'If nRecordsCached < nRecords Then RenderPointsFromFile P ', shapeIndex
    Case 3, 5, 8 'PolyLine, Polygon, MultiPoint
      If nRecordsCached > 0 Then RenderPolysFromCache P, shapeIndex
      'If nRecordsCached < nRecords Then RenderPolysFromFile P ', shapeIndex
  End Select
  P.ForeColor = vbBlack
  P.DrawMode = saveDrawMode
  P.DrawStyle = vbSolid
  P.Refresh
  If shapeIndex > 0 Then pColor(shapeIndex) = saveColor
End Sub

Public Property Get ShapeFile$()
  ShapeFile = pPath & pBaseFilename & ".shp"
End Property

Public Property Get NumRecords() As Long
  NumRecords = nRecords
End Property

'Set the ESRI shape file this layer will be drawn from
'Header information is read for later use
Public Property Let ShapeFile(ByVal newValue$)
  Dim ShapeIO As New CShape_IO
  On Error GoTo NeverMind
  pPath = PathNameOnly(newValue)
  pBaseFilename = FilenameOnly(newValue)
  'If Len(pBaseFilename) > 3 Then
  '  If Mid(pBaseFilename, Len(pBaseFilename) - 3, 1) = "." Then
  '    pBaseFilename = Left(pBaseFilename, Len(pBaseFilename) - 4)
  '  End If
  'End If
  
  ShapeIO.ShapeFileOpen pPath & "\" & pBaseFilename & ".shp", vbReadOnly
  nRecords = ShapeIO.getRecordCount
  nRecordsCached = 0
  header = ShapeIO.getShapeHeader
  If FileLen(pPath & "\" & pBaseFilename & ".shp") < maxCacheableFileSize And nRecords > 0 Then
    ReadIn ShapeIO
  End If
  ShapeIO.FileShutDown
  Exit Property
NeverMind:
  pPath = ""
  pBaseFilename = ""
  nRecords = 0
End Property

'Read drawing information into memory so drawing will be faster
'Reading would be faster if we wrote new routines such as "ShapeIO.getXYPoints" that would get the whole array at once
Private Sub ReadIn(ShapeIO As CShape_IO)
  nRecordsCached = nRecords
  Select Case header.ShapeType
    Case 0 'Null
    Case 1 'Points
      ReDim pPoints(1 To nRecords)
      For Record = 1 To nRecords
        pPoints(Record) = ShapeIO.getXYPoint(Record)
      Next Record
    Case 3, 5, 8 'PolyLine, Polygon, MultiPoint
      ReDim pPolygons(1 To nRecords)
      For Record = 1 To nRecords
        pPolygons(Record) = ShapeIO.getPoly(Record)
      Next Record
    Case Else: 'type not implemented, none read in
      nRecordsCached = 0
  End Select
  ReDim pColor(nRecordsCached)
  ReDim pLastColor(nRecordsCached)
End Sub

Private Sub RenderPoint(P As PictureBox, X As Double, Y As Double, colr As Long)
  P.Circle (X, Y), pRadius, colr
End Sub

Private Sub FillPoly(P As PictureBox, poly As T_shpPoly, colr As Long)
  Dim part As Long, point As Long, maxpoint As Long
  Dim k As Long, b As Long, ob As Long
  Dim polyAPI() As POINTAPI

  With poly
    If .ShapeType = typePolygon Then
      For part = 0 To .NumParts - 1
        If part < .NumParts - 1 Then
          maxpoint = .Parts(part + 1)
        Else
          maxpoint = .NumPoints
        End If
        ReDim polyAPI(0 To maxpoint - point - 1)
        k = 0
        While point < maxpoint
          polyAPI(k).X = ((.thePoints(point).X - pScaleLeft) / pScaleWidth) * pPixelWidth
          polyAPI(k).Y = ((.thePoints(point).Y - pScaleTop) / pScaleHeight) * pPixelHeight
          point = point + 1
          k = k + 1
        Wend
        b = CreateSolidBrush(colr)
        ob = SelectObject(pHdc, b)
        k = Polygon(pHdc, polyAPI(0), UBound(polyAPI))
        k = SelectObject(pHdc, ob)
        k = DeleteObject(b)
      Next part
    End If
  End With
End Sub

'Private Sub RenderPoly(P As PictureBox, poly As T_shpPoly, ByVal colr As Long)
'  Dim part As Long, point As Long, maxpoint As Long
'
'  With poly
'    Select Case .ShapeType
'      Case typeNullShape 'Skip rendering null shapes
'      Case typeMultiPoint
'        For point = 0 To .NumPoints - 1
'          RenderPoint P, .thePoints(point).X, .thePoints(point).Y, colr
'        Next
'      Case Else
'        If .ShapeType = typePolygon And (colr <> 0 Or pAlwaysFillPolygons) Then
'          If colr = 0 Then colr = vbWhite
'          FillPoly P, poly, colr
'          colr = 0
'        Else
'          For part = 0 To .NumParts - 1
'            P.CurrentX = .thePoints(point).X
'            P.CurrentY = .thePoints(point).Y
'            If part < .NumParts - 1 Then
'              maxpoint = .Parts(part + 1)
'            Else
'              maxpoint = .NumPoints
'            End If
'            While point < maxpoint
'              P.Line -(.thePoints(point).X, .thePoints(point).Y), colr
'              point = point + 1
'            Wend
'          Next part
'        End If
'    End Select
'  End With
'End Sub

Private Sub RenderPoly(P As PictureBox, poly As T_shpPoly, ByVal colr As Long)
  Dim part As Long, point As Long, maxpoint As Long
  Dim k As Long, b As Long, ob As Long

  With poly
    Select Case .ShapeType
      Case typeNullShape 'Skip rendering null shapes
      Case typeMultiPoint
        For point = 0 To .NumPoints - 1
          RenderPoint P, .thePoints(point).X, .thePoints(point).Y, colr
        Next
      Case Else
        If .ShapeType = typePolygon And (colr <> 0 Or pAlwaysFillPolygons) Then
          If colr = 0 Then colr = vbWhite
          FillPoly P, poly, colr
          colr = 0
        Else
          b = CreateSolidBrush(colr)
          ob = SelectObject(P.hdc, b)
          For part = 0 To .NumParts - 1
            MoveToEx pHdc, ((.thePoints(point).X - pScaleLeft) / pScaleWidth) * pPixelWidth, _
                            ((.thePoints(point).Y - pScaleTop) / pScaleHeight) * pPixelHeight, vbNull
            If part < .NumParts - 1 Then
              maxpoint = .Parts(part + 1)
            Else
              maxpoint = .NumPoints
            End If
            While point < maxpoint
              LineTo pHdc, ((.thePoints(point).X - pScaleLeft) / pScaleWidth) * pPixelWidth, _
                            ((.thePoints(point).Y - pScaleTop) / pScaleHeight) * pPixelHeight
              point = point + 1
            Wend
          Next part
          k = SelectObject(pHdc, ob)
          k = DeleteObject(b)
        End If
    End Select
  End With
End Sub

'Private Sub RenderPointsFromFile(P As PictureBox)
'  Dim ShapeIO As New CShape_IO
'  Dim aXYPoint As ShapeDefines.T_shpXYPoint
'
'  ShapeIO.ShapeFileOpen pPath & "\" & pBaseFilename & ".shp", vbReadOnly
'  While Record <= nRecords 'And pRendering > 0
'    aXYPoint = ShapeIO.getXYPoint(Record)
'    If aXYPoint.ShapeType = typePoint Then RenderPoint P, aXYPoint.thePoint.X, aXYPoint.thePoint.Y, pColor(Record)
'    Record = Record + 1
'  Wend
'  ShapeIO.FileShutDown
'End Sub

Private Sub RenderPointsFromCache(P As PictureBox, iShape As Long)
  On Error Resume Next
  If iShape > 0 Then
    RenderPoint P, pPoints(iShape).thePoint.X, pPoints(iShape).thePoint.Y, pColor(iShape)
  Else
    While Record <= nRecordsCached 'And pRendering > 0
      'Debug.Print pPoints(record).thePoint.x, pPoints(record).thePoint.y
      RenderPoint P, pPoints(Record).thePoint.X, pPoints(Record).thePoint.Y, pColor(Record)
      Record = Record + 1
    Wend
  End If
End Sub

'Renders pPolygons, ploylines, and multipoints
'Private Sub RenderPolysFromFile(P As PictureBox)
'  Dim ShapeIO As New CShape_IO
'  ShapeIO.ShapeFileOpen pPath & "\" & pBaseFilename & ".shp", vbReadOnly
'  While Record <= nRecords 'And pRendering > 0
'    RenderPoly P, ShapeIO.getPoly(Record), pColor(Record)
'    Record = Record + 1
'  Wend
'  ShapeIO.FileShutDown
'End Sub

Private Sub RenderPolysFromCache(P As PictureBox, iShape As Long)
  If iShape > 0 Then
    RenderPoly P, pPolygons(iShape), pColor(iShape)
  Else
    While Record <= nRecordsCached 'And pRendering > 0)
      RenderPoly P, pPolygons(Record), pColor(Record)
      Record = Record + 1
    Wend
  End If
End Sub

Public Property Get LabelField() As String
  If Len(pLabelField) > 0 Then
    LabelField = pLabelField
  Else
    Select Case LCase(FilenameOnly(pBaseFilename))
      Case "cat":      LabelField = "NAME"
      Case "catpt":    LabelField = "HUC"
      Case "cerclis":  LabelField = "FACILITY_N"
      Case "cnty":     LabelField = "CNTYNAME"
      Case "cntypt":   LabelField = "CNTYNAME"
      Case "dam":      LabelField = "DAM_NAME"
      Case "dws":      LabelField = "NAME"
      Case "ecoreg":   LabelField = "NAME"
      Case "fhards":   LabelField = "LNAME"
      Case "gage":     LabelField = "NAME"
      Case "gappl":    LabelField = "NAME"
      Case "ifd":      LabelField = "NAM"
      Case "lulcndx":  LabelField = "QNAME"
      Case "mad":      LabelField = "AREANAME"
      Case "met_stat": LabelField = "STATNAME"
      Case "metpt":    LabelField = "STATNAME"
      Case "mines":    LabelField = "NAME"
      Case "nawqa":    LabelField = "NAME"
      Case "nsi":      LabelField = "LOCATION"
      Case "pcs3":     LabelField = "FAC_NAME"
      Case "rcris":    LabelField = "FACILITY_N"
      Case "rf1":      LabelField = "PNAME"
      Case "st":       LabelField = "ST"
      Case "statsgo":  LabelField = "MUID"
      Case "tnppl":    LabelField = "NAME"
      Case "tri":      LabelField = "FACILITY_N"
      Case "urban":    LabelField = "CITYNAME"
      Case "urban_nm": LabelField = "CITYNAME"
      Case "wdm":      LabelField = "STA_NAME"
      Case "wq_stat":  LabelField = "LOCATION"
      Case "wqobs":    LabelField = "LOCATION"
      Case Else:       LabelField = "NAME"
    End Select
  End If
End Property
