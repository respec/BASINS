Option Strict Off
Option Explicit On

Imports MapWinUtility

Module modShapeDump
	
    Public Sub ShapeDump(ByVal shpFileNames As ArrayList)
        Dim shpIn As CShape_IO
        Dim curBaseName As String
        Dim curShpName As String
        Dim saveLogFilename As String = Logger.FileName

        For Each lFilename As String In shpFileNames
            curBaseName = FilenameNoExt(lFilename)
            If Len(curBaseName) > 0 Then
                curShpName = curBaseName & ".shp"
                shpIn = New CShape_IO
                If Not IO.File.Exists(curShpName) Then
                    Logger.Dbg("ShapeDump could not find '" & curShpName & "'")
                ElseIf Not shpIn.ShapeFileOpen(curShpName, CShape_IO.READWRITEFLAG.Readwrite) Then
                    Logger.Dbg("ShapeDump Could not open '" & curShpName & "'")
                Else
                    '    pLogger.SetFileName(curBaseName & ".dump.txt")
                    Dim lDumpFile As New IO.StreamWriter(curBaseName & ".dump.txt", IO.FileMode.Create)
                    lDumpFile.WriteLine("Dumping " & shpIn.getRecordCount & " records from " & curShpName)
                    With shpIn.getShapeHeader
                        lDumpFile.WriteLine("----------File Header----------")
                        lDumpFile.WriteLine("FileCode: " & .FileCode)
                        lDumpFile.WriteLine("unused: " & .u1 & ", " & .u2 & ", " & .u3 & ", " & .u4 & ", " & .u5)

                        lDumpFile.WriteLine("FileLength: " & .FileLength)
                        lDumpFile.WriteLine("Version: " & .version)
                        lDumpFile.WriteLine("ShapeType: " & .ShapeType & " = " & shpIn.getShapeName)

                        lDumpFile.WriteLine("X range: " & .BndBoxXmin & " to " & .BndBoxXmax)
                        lDumpFile.WriteLine("Y range: " & .BndBoxYmin & " to " & .BndBoxYmax)
                        lDumpFile.WriteLine("Z range: " & .BndBoxZmin & " to " & .BndBoxZmax)
                        lDumpFile.WriteLine("M range: " & .BndBoxMmin & " to " & .BndBoxMmax)
                    End With
                    lDumpFile.WriteLine("----------Shapes----------")
                    Select Case shpIn.getShapeHeader.ShapeType
                        Case CShape_IO.FILETYPEENUM.typePoint : DumpPoints(shpIn, lDumpFile)
                            'TODO: typePointM, typePointZ
                        Case CShape_IO.FILETYPEENUM.typePolyline, CShape_IO.FILETYPEENUM.typePolygon, CShape_IO.FILETYPEENUM.typeMultipoint, _
                             CShape_IO.FILETYPEENUM.typePolyLineZ, CShape_IO.FILETYPEENUM.typePolygonZ, CShape_IO.FILETYPEENUM.typeMultiPointZ, _
                             CShape_IO.FILETYPEENUM.typePolyLineM, CShape_IO.FILETYPEENUM.typePolygonM, CShape_IO.FILETYPEENUM.typeMultiPointM, _
                             CShape_IO.FILETYPEENUM.typeMultiPatch
                            DumpPolys(shpIn, lDumpFile)
                        Case Else
                            Logger.Msg("Unable to dump shape type " & shpIn.getShapeHeader.ShapeType, "ShapeProject")
                            Exit Sub
                    End Select
                    lDumpFile.Close()
                End If
                shpIn.FileShutDown()
                shpIn = Nothing
            End If
        Next

    End Sub
	
    Private Sub DumpPoints(ByRef shpIn As CShape_IO, ByVal aStream As IO.StreamWriter)
        Dim record, nRecords As Integer
        Dim aXYPoint As ShapeDefines.T_shpXYPoint

        nRecords = shpIn.getRecordCount
        For record = 1 To nRecords
            aXYPoint = shpIn.getXYPoint(record)
            aStream.WriteLine(aXYPoint.thePoint.x & " " & aXYPoint.thePoint.y)
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
    '      Logger.Dbg "MultiPoint " & record & ", " & .NumPoints & " points"
    '      Logger.Dbg "Box Min: (" & .Box.xMin & ", " & .Box.yMin & ")"
    '      Logger.Dbg "Box Max: (" & .Box.xMax & ", " & .Box.yMax & ")"
	'      For point = 0 To .NumPoints - 1
    '        Logger.Dbg .thePoints(point).x & " " & .thePoints(point).y
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
    '      Logger.Dbg "Polyline " & record & ", " & .NumParts & " parts, " & .NumPoints & " points"
    '      Logger.Dbg "Box Min: (" & .Box.xMin & ", " & .Box.yMin & ")"
    '      Logger.Dbg "Box Max: (" & .Box.xMax & ", " & .Box.yMax & ")"
	'      For part = 0 To .NumParts - 1
	'        If part < .NumParts - 1 Then
	'          maxpoint = .Parts(part + 1)
	'        Else
	'          maxpoint = .NumPoints
	'        End If
	'        While point < maxpoint
    '          Logger.Dbg .thePoints(point).x & " " & .thePoints(point).y
	'          point = point + 1
	'        Wend
	'      Next part
	'    End With
	'  Next
	'End Sub
	
    Private Sub DumpPolys(ByRef shpIn As CShape_IO, ByVal aStream As IO.StreamWriter)
        Dim record, nRecords As Integer
        Dim lPoly As ShapeDefines.T_shpPoly
        Dim point, part, maxpoint As Integer

        nRecords = shpIn.getRecordCount
        For record = 1 To nRecords
            lPoly = shpIn.getPoly(record)
            point = 0
            With lPoly
                aStream.WriteLine("Shape " & record & "---------------" & .NumPoints & " points")
                aStream.WriteLine("X range (" & .Box.xMin & ", " & .Box.xMax & ")")
                aStream.WriteLine("Y range (" & .Box.yMin & ", " & .Box.yMax & ")")

                Select Case .ShapeType 'shapes with M
                    Case CShape_IO.FILETYPEENUM.typePolyLineZ, CShape_IO.FILETYPEENUM.typePolygonZ, CShape_IO.FILETYPEENUM.typeMultiPointZ, CShape_IO.FILETYPEENUM.typePolyLineM, CShape_IO.FILETYPEENUM.typePolygonM, CShape_IO.FILETYPEENUM.typeMultiPointM, CShape_IO.FILETYPEENUM.typeMultiPatch
                        aStream.WriteLine("M range (" & .Mmin & ", " & .Mmax & ")")
                End Select

                Select Case .ShapeType 'shapes with Z
                    Case CShape_IO.FILETYPEENUM.typePolyLineZ, CShape_IO.FILETYPEENUM.typePolygonZ, CShape_IO.FILETYPEENUM.typeMultiPointZ, CShape_IO.FILETYPEENUM.typeMultiPatch
                        aStream.WriteLine("Z range (" & .Zmin & ", " & .Zmax & ")")
                End Select

                Select Case .ShapeType 'MultiPoint types have no parts
                    Case CShape_IO.FILETYPEENUM.typeMultipoint, CShape_IO.FILETYPEENUM.typeMultiPointZ, CShape_IO.FILETYPEENUM.typeMultiPointM
                        While point < maxpoint
                            Select Case .ShapeType 'Read Z values for shapes with Z
                                Case CShape_IO.FILETYPEENUM.typeMultiPointZ
                                    aStream.WriteLine(.thePoints(point).x & " " & .thePoints(point).y & " " & .Zarray(point))
                                Case CShape_IO.FILETYPEENUM.typeMultiPointM
                                    aStream.WriteLine(.thePoints(point).x & " " & .thePoints(point).y & " " & .Zarray(point) & " " & .Marray(point))
                                Case CShape_IO.FILETYPEENUM.typeMultipoint
                                    aStream.WriteLine(.thePoints(point).x & " " & .thePoints(point).y)
                            End Select
                            point = point + 1
                        End While

                    Case Else
                        aStream.WriteLine("Parts: " & .NumParts)
                        For part = 0 To .NumParts - 1
                            If part < .NumParts - 1 Then
                                maxpoint = .Parts(part + 1)
                            Else
                                maxpoint = .NumPoints
                            End If
                            aStream.WriteLine("Part " & part & ", " & maxpoint - point & " points")
                            If .ShapeType = CShape_IO.FILETYPEENUM.typeMultiPatch Then aStream.WriteLine("PartType: " & .PartTypes(part))
                            While point < maxpoint
                                Select Case .ShapeType 'Read Z values for shapes with Z
                                    Case CShape_IO.FILETYPEENUM.typePolyLineZ, CShape_IO.FILETYPEENUM.typePolygonZ, CShape_IO.FILETYPEENUM.typeMultiPointZ
                                        aStream.WriteLine(.thePoints(point).x & " " & .thePoints(point).y & " Z= " & .Zarray(point) & " M= " & .Marray(point))
                                    Case CShape_IO.FILETYPEENUM.typePolyLineM, CShape_IO.FILETYPEENUM.typePolygonM, CShape_IO.FILETYPEENUM.typeMultiPointM, CShape_IO.FILETYPEENUM.typeMultiPatch
                                        aStream.WriteLine(.thePoints(point).x & " " & .thePoints(point).y & " M= " & .Marray(point))
                                    Case Else
                                        aStream.WriteLine(.thePoints(point).x & " " & .thePoints(point).y)
                                End Select
                                point = point + 1
                            End While
                        Next part
                End Select
            End With
        Next
        Exit Sub

    End Sub
End Module