Option Strict Off
Option Explicit On

Imports atcUtility

Module ShapeDefines
	'*****************************************************************************************
	'ShapeDefines
	'
	'Contains all of the user defined types for the valid shape file types
	
	
	'*****************************************************************************************
	'Main file header stucture for both the shape and index files
	Public Structure T_MainFileHeader
		Dim FileCode As Integer 'big
		Dim u1 As Integer 'big
		Dim u2 As Integer 'big
		Dim u3 As Integer 'big
		Dim u4 As Integer 'big
		Dim u5 As Integer 'big
		Dim FileLength As Integer 'big
		Dim version As Integer 'Little
		Dim ShapeType As Integer 'Little
		Dim BndBoxXmin As Double 'Little
		Dim BndBoxYmin As Double 'Little
		Dim BndBoxXmax As Double 'Little
		Dim BndBoxYmax As Double 'Little
		Dim BndBoxZmin As Double 'Little
		Dim BndBoxZmax As Double 'Little
		Dim BndBoxMmin As Double 'Little
		Dim BndBoxMmax As Double 'Little
	End Structure
	
	'*****************************************************************************************
	'Shape file record header structure
	Public Structure T_RecordHeader
		Dim RecordNumber As Integer 'big
		Dim ContentLength As Integer 'big
	End Structure
	
	'*****************************************************************************************
	'index file record structure
	Public Structure T_IndexRecordHeader
		Dim offset As Integer 'big
		Dim ContentLength As Integer 'big
	End Structure
	
	'*****************************************************************************************
	'Structure for the null shape
	Public Structure T_NullShape
		Dim ShapeType As Integer
	End Structure
	
	'*****************************************************************************************
	' Basic point structure for standard point line and polygon types
	Public Structure T_BasicPoint
		Dim x As Double
		Dim y As Double
	End Structure
	'*****************************************************************************************
	'structure for the bounding box
	Public Structure T_BoundingBox
		Dim xMin As Double
		Dim yMin As Double
		Dim xMax As Double
		Dim yMax As Double
	End Structure
	
	'*****************************************************************************************
	'structure for the XYPoint shapetype
	Public Structure T_shpXYPoint
		Dim ShapeType As Integer
		Dim thePoint As T_BasicPoint
	End Structure
	
	'*****************************************************************************************
	'structure for the MultiPoint, polyline, polygon, MultiPatch shape types
	Public Structure T_shpPoly
		Dim ShapeType As Integer
		Dim Box As T_BoundingBox
		Dim NumParts As Integer 'Always 0 for XYMultiPoint
		Dim NumPoints As Integer
		Dim Parts() As Integer 'Always empty for MultiPoint types
		Dim PartTypes() As Integer 'Only used by MultiPatch
		Dim thePoints() As T_BasicPoint
		'Z and M not used by simple PolyLine, Polygon, MultiPoint
		Dim Zmin As Double
		Dim Zmax As Double
		Dim Zarray() As Double
		Dim Mmin As Double
		Dim Mmax As Double
		Dim Marray() As Double
	End Structure
	
	'*****************************************************************************************
	'structure for the pointM and pointZ types
	Public Structure T_shpPointMZ
		Dim ShapeType As Integer
		Dim thePoint As T_BasicPoint
		Dim Z As Double
		Dim M As Double
	End Structure
	
	Public Function PointInPolygon(ByRef x As Double, ByRef y As Double, ByRef poly As T_shpPoly) As Boolean
		Dim iPart As Integer
		Dim iPoint As Integer
		Dim iMaxPoint As Integer
		Dim pt1 As T_BasicPoint
		Dim pt2 As T_BasicPoint
        With poly
            'First, it has to be within bounding box.
            If x >= .Box.xMin And x <= .Box.xMax And y >= .Box.yMin And y <= .Box.yMax Then

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
                    End While
                Next
            Else 'Not in bounding box, so not within poly
                PointInPolygon = False
            End If
        End With
	End Function
	
	'True if any segment of poly intersects the segment (X1, Y1)-(X2, Y2)
	Public Function LineIntersectsPolygon(ByRef X1 As Double, ByRef Y1 As Double, ByRef X2 As Double, ByRef Y2 As Double, ByRef poly As T_shpPoly) As Boolean
		Dim iPart As Integer
		Dim iPoint As Integer
		Dim iMaxPoint As Integer
		Dim pt1 As T_BasicPoint
		Dim pt2 As T_BasicPoint
        Dim dxLine As Double
		Dim dyLine As Double
		Dim dxPoly As Double
		Dim dyPoly As Double
		Dim denom As Double
		Dim dy1 As Double
		Dim dx1 As Double
		Dim s As Double
		Dim t As Double
		With poly
			
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
				End While
			Next 
		End With
	End Function
	
	Public Function AnyPointInPolygon(ByRef aPolyPoints As T_shpPoly, ByRef aPolygon As T_shpPoly) As Boolean
        Dim iPoint As Integer
		Dim iMaxPoint As Integer
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
		End While
		AnyPointInPolygon = False
	End Function
	
	Private Function AnyLineIntersectsPolygon(ByRef aPolyLine As T_shpPoly, ByRef aPolygon As T_shpPoly) As Boolean
        Dim iPoint As Integer
		Dim iMaxPoint As Integer
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
		End While
		'For polygon types, check implied edge back to first point
		Select Case aPolyLine.ShapeType
			Case CShape_IO.FILETYPEENUM.typePolygon, CShape_IO.FILETYPEENUM.typePolygonZ, CShape_IO.FILETYPEENUM.typePolygonM
                pt1 = aPolyLine.thePoints(0)
				If LineIntersectsPolygon(pt1.x, pt1.y, pt2.x, pt2.y, aPolygon) Then
					AnyLineIntersectsPolygon = True
				End If
		End Select
	End Function
	
	Public Function PolyLineOverlapsPolygon(ByRef aPolyLine As T_shpPoly, ByRef aPolygon As T_shpPoly) As Boolean
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
	Function PolygonsOverlap(ByRef aPoly1 As T_shpPoly, ByRef aPoly2 As T_shpPoly) As Boolean
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
    Public Sub WriteShapePointsFromDBF(ByRef aDBF As atcTableDBF, ByRef aLatitudeYfield As Integer, ByRef aLongitudeXfield As Integer)
        Dim shp As CShape_IO
        Dim XYPoint As T_shpXYPoint
        Dim iRecord As Integer
        Dim vCoordinate As String
        Dim shpFilename As String
        Dim nNonNumericLat As Integer
        Dim nNonNumericLon As Integer

        XYPoint.ShapeType = CShape_IO.FILETYPEENUM.typePoint 'typePoint = 1
        shp = New CShape_IO
        shpFilename = IO.Path.ChangeExtension(aDBF.FileName, ".shp")
        shp.CreateNewShape(shpFilename, XYPoint.ShapeType)
        For iRecord = 1 To aDBF.NumRecords
            aDBF.CurrentRecord = iRecord
            vCoordinate = aDBF.Value(aLongitudeXfield)
            If IsNumeric(vCoordinate) Then
                XYPoint.thePoint.x = CDbl(vCoordinate)
            Else
                XYPoint.thePoint.x = 0
                nNonNumericLon = nNonNumericLon + 1
                IO.File.AppendAllText(IO.Path.ChangeExtension(shpFilename, ".err"), "Non-numeric longitude at " & iRecord & ": " & vCoordinate & vbCrLf)
            End If

            vCoordinate = aDBF.Value(aLatitudeYfield)
            If IsNumeric(vCoordinate) Then
                XYPoint.thePoint.y = CDbl(vCoordinate)
            Else
                XYPoint.thePoint.y = 0
                nNonNumericLat = nNonNumericLat + 1
                IO.File.AppendAllText(IO.Path.ChangeExtension(shpFilename, ".err"), "Non-numeric latitude at " & iRecord & ": " & vCoordinate & vbCrLf)
            End If
            shp.putXYPoint(0, XYPoint)
        Next
        shp.FileShutDown()
        If nNonNumericLon > 0 Then System.Diagnostics.Debug.WriteLine(nNonNumericLon & " non-numeric longitudes in " & shpFilename)
        If nNonNumericLat > 0 Then System.Diagnostics.Debug.WriteLine(nNonNumericLat & " non-numeric latitudes in " & shpFilename)
    End Sub
End Module