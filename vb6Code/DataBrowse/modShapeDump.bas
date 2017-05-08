Attribute VB_Name = "modShapeDump"
Option Explicit

'Return a text description of the shapes in the file named
Public Function ShapeDump(shpFileName As String) As String
  Dim shpIn As New CShape_IO
  Dim firstInput As Long
  Dim lastInput As Long
  Dim curInput As Long
  Dim curBaseName As String
  Dim curShpName As String
  Dim findPos As Long
  Dim dumpFilename As String
    
  curBaseName = FilenameNoExt(shpFileName)
  curShpName = curBaseName & ".shp"
  dumpFilename = curBaseName & ".dumptemp"

  If Len(curBaseName) = 0 Then
    ShapeDump = ""
  ElseIf Not shpIn.ShapeFileOpen(curShpName, READWRITEFLAG.Readwrite) Then
    ShapeDump = "Could not open '" & curShpName & "'" & vbCrLf
  Else
    AppendFileString dumpFilename, "Reading " & shpIn.getRecordCount _
                                        & " shapes in " & FilenameOnly(curBaseName) & vbCrLf
    
    Select Case shpIn.getShapeHeader.ShapeType
      Case typePoint:      DumpPoints shpIn, dumpFilename
      Case typeMultiPoint: DumpMultiPoints shpIn, dumpFilename
      Case typepolyline:   DumpPolylines shpIn, dumpFilename
      Case typepolygon:    DumpPolygons shpIn, dumpFilename
      Case Else
        AppendFileString dumpFilename, "Unsupported shape type " _
                                & shpIn.getShapeHeader.ShapeType & vbCrLf
        Exit Function
    End Select
    
    shpIn.FileShutDown
    Set shpIn = Nothing
    ShapeDump = WholeFileString(dumpFilename)
    Kill dumpFilename
  End If
End Function

Private Sub DumpPoints(shpIn As CShape_IO, dumpFilename As String)
  Dim record As Long, nRecords As Long
  Dim aXYPoint As ShapeDefines.T_shpXYPoint
  
  nRecords = shpIn.getRecordCount
  For record = 1 To nRecords
    aXYPoint = shpIn.getXYPoint(record)
    AppendFileString dumpFilename, aXYPoint.thePoint.x & " " & aXYPoint.thePoint.y & vbCrLf
  Next
End Sub

Private Sub DumpMultiPoints(shpIn As CShape_IO, dumpFilename As String)
  Dim record As Long, nRecords As Long
  Dim aXYMultiPoint As ShapeDefines.T_shpXYMultiPoint
  Dim point&
  
  nRecords = shpIn.getRecordCount
  For record = 1 To nRecords
    aXYMultiPoint = shpIn.getXYMultiPoint(record)
    With aXYMultiPoint
      AppendFileString dumpFilename, "MultiPoint " & record & ", " & .NumPoints & " points" & vbCrLf
      AppendFileString dumpFilename, "Box Min: (" & .Box.xMin & ", " & .Box.yMin & ")" & vbCrLf
      AppendFileString dumpFilename, "Box Max: (" & .Box.xMax & ", " & .Box.yMax & ")" & vbCrLf
      For point = 0 To .NumPoints - 1
        AppendFileString dumpFilename, .thePoints(point).x & " " & .thePoints(point).y & vbCrLf
      Next point
    End With
  Next
End Sub

Private Sub DumpPolylines(shpIn As CShape_IO, dumpFilename As String)
  Dim record As Long, nRecords As Long
  Dim aPolyLine As ShapeDefines.T_shpPolyLine
  Dim part&, point&, maxpoint&
  
  nRecords = shpIn.getRecordCount
  For record = 1 To nRecords
    aPolyLine = shpIn.getPolyLine(record)
    point = 0
    With aPolyLine
      AppendFileString dumpFilename, "Polyline " & record & ", " _
                       & .NumParts & " parts, " & .NumPoints & " points" & vbCrLf
      AppendFileString dumpFilename, "Box Min: (" & .Box.xMin & ", " & .Box.yMin & ")" & vbCrLf
      AppendFileString dumpFilename, "Box Max: (" & .Box.xMax & ", " & .Box.yMax & ")" & vbCrLf
      For part = 1 To .NumParts
        If part < .NumParts - 1 Then
          maxpoint = .Parts(part)
        Else
          maxpoint = .NumPoints
        End If
        AppendFileString dumpFilename, "Part " & part & ", " & maxpoint - point & " points" & vbCrLf
        While point < maxpoint
          AppendFileString dumpFilename, .thePoints(point).x & " " & .thePoints(point).y & vbCrLf
          point = point + 1
        Wend
      Next part
    End With
  Next
End Sub

Private Sub DumpPolygons(shpIn As CShape_IO, dumpFilename As String)
  Dim record As Long, nRecords As Long
  Dim aPolygon As ShapeDefines.T_shpPolygon
  Dim part&, point&, maxpoint&
  
  nRecords = shpIn.getRecordCount
  For record = 1 To nRecords
    aPolygon = shpIn.getPolygon(record)
    point = 0
    With aPolygon
      AppendFileString dumpFilename, "Polygon " & record & ", " _
                       & .NumParts & " parts, " & .NumPoints & " points" & vbCrLf
      AppendFileString dumpFilename, "Box Min: (" & .Box.xMin & ", " & .Box.yMin & ")" & vbCrLf
      AppendFileString dumpFilename, "Box Max: (" & .Box.xMax & ", " & .Box.yMax & ")" & vbCrLf
      For part = 1 To .NumParts
        If part < .NumParts Then
          maxpoint = .Parts(part)
        Else
          maxpoint = .NumPoints
        End If
        AppendFileString dumpFilename, "Part " & part & ", " & maxpoint - point & " points" & vbCrLf
        While point < maxpoint
          AppendFileString dumpFilename, .thePoints(point).x & " " & .thePoints(point).y & vbCrLf
          point = point + 1
        Wend
      Next part
    End With
  Next
End Sub



