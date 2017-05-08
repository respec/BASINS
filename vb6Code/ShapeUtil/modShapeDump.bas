Attribute VB_Name = "modShapeDump"
Option Explicit

Public Sub ShapeDump(shpFileNames() As String)
  Dim shpIn As CShape_IO
  Dim firstInput As Long
  Dim lastInput As Long
  Dim curInput As Long
  Dim curBaseName As String
  Dim curShpName As String
  Dim findPos As Long
  Dim shpheader As T_MainFileHeader
  Dim saveLogFilename As String
  Dim pLogger As clsATCoLogger
    
  firstInput = LBound(shpFileNames)
  lastInput = UBound(shpFileNames)

  saveLogFilename = gLogger.GetFileName
  
  For curInput = firstInput To lastInput
    curBaseName = FilenameNoExt(shpFileNames(curInput))
    If Len(curBaseName) > 0 Then
      curShpName = curBaseName & ".shp"
      Set shpIn = New CShape_IO
      If Not FileExists(curShpName) Then
        gLogger.Log "ShapeDump could not find '" & curShpName & "'"
      ElseIf Not shpIn.ShapeFileOpen(curShpName, READWRITEFLAG.Readwrite) Then
        gLogger.Log "ShapeDump Could not open '" & curShpName & "'"
      Else
        Set pLogger = New clsATCoLogger
        pLogger.SetFileName curBaseName & ".dump.txt"
        pLogger.Log "Dumping " & shpIn.getRecordCount & " records from " & curShpName
        gLogger.Log "Dumping " & shpIn.getRecordCount & " records from " & curShpName & " in " & pLogger.GetFileName
        shpheader = shpIn.getShapeHeader
        
        pLogger.Log "----------File Header----------"
        pLogger.Log "FileCode: " & shpheader.FileCode
        pLogger.Log "unused: " & shpheader.u1 & ", " _
                               & shpheader.u2 & ", " _
                               & shpheader.u3 & ", " _
                               & shpheader.u4 & ", " _
                               & shpheader.u5
        
        pLogger.Log "FileLength: " & shpheader.FileLength
        pLogger.Log "Version: " & shpheader.version
        pLogger.Log "ShapeType: " & shpheader.ShapeType & " = " & shpIn.getShapeName
        
        pLogger.Log "X range: " & shpheader.BndBoxXmin & " to " & shpheader.BndBoxXmax
        pLogger.Log "Y range: " & shpheader.BndBoxYmin & " to " & shpheader.BndBoxYmax
        pLogger.Log "Z range: " & shpheader.BndBoxZmin & " to " & shpheader.BndBoxZmax
        pLogger.Log "M range: " & shpheader.BndBoxMmin & " to " & shpheader.BndBoxMmax
        
        pLogger.Log "----------Shapes----------"
        Select Case shpIn.getShapeHeader.ShapeType
          Case typePoint:      DumpPoints shpIn, pLogger
          'TODO: typePointM, typePointZ
          Case typePolyline, typePolygon, typeMultipoint, _
               typePolyLineZ, typePolygonZ, typeMultiPointZ, _
               typePolyLineM, typePolygonM, typeMultiPointM, typeMultiPatch
            DumpPolys shpIn, pLogger
          Case Else
            pLogger.LogMsg "Unable to dump shape type " & shpIn.getShapeHeader.ShapeType, "ShapeProject"
            Exit Sub
        End Select
        Set pLogger = Nothing
      End If
      shpIn.FileShutDown
      Set shpIn = Nothing
      gLogger.Log "ShapeDump Finished with " & curBaseName
    End If
  Next

End Sub

Private Sub DumpPoints(shpIn As CShape_IO, aLogger As clsATCoLogger)
  Dim record As Long, nRecords As Long
  Dim aXYPoint As ShapeDefines.T_shpXYPoint
  
  nRecords = shpIn.getRecordCount
  For record = 1 To nRecords
    aXYPoint = shpIn.getXYPoint(record)
    aLogger.Log aXYPoint.thePoint.x & " " & aXYPoint.thePoint.y
  Next
End Sub

'Private Sub DumpMultiPoints(shpIn As CShape_IO)
'  Dim record As Long, nRecords As Long
'  Dim aXYMultiPoint As ShapeDefines.T_shpXYMultiPoint
'  Dim point&
'
'  nRecords = shpIn.getRecordCount
'  For record = 1 To nRecords
'    aXYMultiPoint = shpIn.getXYMultiPoint(record)
'    With aXYMultiPoint
'      gLogger.Log "MultiPoint " & record & ", " & .NumPoints & " points"
'      gLogger.Log "Box Min: (" & .Box.xMin & ", " & .Box.yMin & ")"
'      gLogger.Log "Box Max: (" & .Box.xMax & ", " & .Box.yMax & ")"
'      For point = 0 To .NumPoints - 1
'        gLogger.Log .thePoints(point).x & " " & .thePoints(point).y
'      Next point
'    End With
'  Next
'End Sub

'Private Sub DumpPolylines(shpIn As CShape_IO)
'  Dim record As Long, nRecords As Long
'  Dim aPolyLine As ShapeDefines.T_shpPolyLine
'  Dim part&, point&, maxpoint&
'
'  nRecords = shpIn.getRecordCount
'  For record = 1 To nRecords
'    aPolyLine = shpIn.getPolyLine(record)
'    point = 0
'    With aPolyLine
'      gLogger.Log "Polyline " & record & ", " & .NumParts & " parts, " & .NumPoints & " points"
'      gLogger.Log "Box Min: (" & .Box.xMin & ", " & .Box.yMin & ")"
'      gLogger.Log "Box Max: (" & .Box.xMax & ", " & .Box.yMax & ")"
'      For part = 0 To .NumParts - 1
'        If part < .NumParts - 1 Then
'          maxpoint = .Parts(part + 1)
'        Else
'          maxpoint = .NumPoints
'        End If
'        While point < maxpoint
'          gLogger.Log .thePoints(point).x & " " & .thePoints(point).y
'          point = point + 1
'        Wend
'      Next part
'    End With
'  Next
'End Sub

Private Sub DumpPolys(shpIn As CShape_IO, aLogger As clsATCoLogger)
  Dim record As Long, nRecords As Long
  Dim lPoly As ShapeDefines.T_shpPoly
  Dim part&, point&, maxpoint&
  
  nRecords = shpIn.getRecordCount
  For record = 1 To nRecords
    lPoly = shpIn.getPoly(record)
    point = 0
    With lPoly
      aLogger.Log "Shape " & record & "---------------" & .NumPoints & " points"
      aLogger.Log "X range (" & .Box.xMin & ", " & .Box.xMax & ")"
      aLogger.Log "Y range (" & .Box.yMin & ", " & .Box.yMax & ")"
      
      Select Case .ShapeType  'shapes with M
        Case typePolyLineZ, typePolygonZ, typeMultiPointZ, _
             typePolyLineM, typePolygonM, typeMultiPointM, typeMultiPatch
          aLogger.Log "M range (" & .Mmin & ", " & .Mmax & ")"
      End Select
      
      Select Case .ShapeType  'shapes with Z
        Case typePolyLineZ, typePolygonZ, typeMultiPointZ, typeMultiPatch
          aLogger.Log "Z range (" & .Zmin & ", " & .Zmax & ")"
      End Select
      
      Select Case .ShapeType 'MultiPoint types have no parts
        Case typeMultipoint, typeMultiPointZ, typeMultiPointM
          While point < maxpoint
            Select Case .ShapeType  'Read Z values for shapes with Z
              Case typeMultiPointZ
                aLogger.Log .thePoints(point).x & " " & .thePoints(point).y & " " & .Zarray(point)
              Case typeMultiPointM
                aLogger.Log .thePoints(point).x & " " & .thePoints(point).y & " " & .Zarray(point) & " " & .Marray(point)
              Case typeMultipoint
                aLogger.Log .thePoints(point).x & " " & .thePoints(point).y
            End Select
            point = point + 1
          Wend
        
        Case Else
          aLogger.Log "Parts: " & .NumParts
          For part = 0 To .NumParts - 1
            If part < .NumParts - 1 Then
              maxpoint = .Parts(part + 1)
            Else
              maxpoint = .NumPoints
            End If
            aLogger.Log "Part " & part & ", " & maxpoint - point & " points"
            If .ShapeType = typeMultiPatch Then aLogger.Log "PartType: " & .PartTypes(part)
            While point < maxpoint
              Select Case .ShapeType  'Read Z values for shapes with Z
                Case typePolyLineZ, typePolygonZ, typeMultiPointZ
                  aLogger.Log .thePoints(point).x & " " & .thePoints(point).y & " Z= " & .Zarray(point) & " M= " & .Marray(point)
                Case typePolyLineM, typePolygonM, typeMultiPointM, typeMultiPatch
                  aLogger.Log .thePoints(point).x & " " & .thePoints(point).y & " M= " & .Marray(point)
                Case Else
                  aLogger.Log .thePoints(point).x & " " & .thePoints(point).y
              End Select
              point = point + 1
            Wend
          Next part
      End Select
    End With
  Next
  Exit Sub
  
End Sub



