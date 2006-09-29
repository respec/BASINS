Option Strict Off
Option Explicit On

Imports MapWinUtility

Module modShapeProject
	
	Private Declare Function SetEnvironmentVariable Lib "kernel32"  Alias "SetEnvironmentVariableA"(ByVal lpName As String, ByVal lpValue As String) As Integer
	
	' proj.dll declarations
	Private Declare Function pj_init_plus Lib "projATC.dll" (ByVal init As String) As Integer
	Private Declare Sub pj_free Lib "projATC.dll" (ByVal pointer As Integer)
	Private Declare Function pj_transform Lib "projATC.dll" (ByVal SrcPrj As Integer, ByVal DestPrj As Integer, ByVal nPoints As Integer, ByVal offset As Integer, ByRef x As Double, ByRef y As Double, ByRef Z As Double) As Integer
	
	Private SrcPrj As Integer 'Pointer to source projection structure in proj.dll
	Private DestPrj As Integer 'Pointer to destination projection structure in proj.dll
	Private Const DegreesToRadians As Double = 0.01745329252
	Private ConvertToRadians As Boolean
	
    Public Sub ShapeProject(ByRef projectionDest As String, ByRef projectionSource As String, ByRef shpFileNames As ArrayList)
        Dim lFilename As String
        Dim shpIn As New CShape_IO
        Dim shpOut As New CShape_IO
        Dim curBaseName As String
        Dim curShpName As String
        Dim projCmdLine As String
        Dim findPos As Integer
        Dim tmpSHPname As String
        Dim tmpSHXname As String

        On Error GoTo ErrHand

        projCmdLine = projInitFromFile(projectionDest)
        Logger.Dbg("projectionDest = " & projCmdLine)
        If InStr(projCmdLine, "proj=dd ") > 0 Or InStr(projCmdLine, "proj=latlong ") > 0 Then
            Logger.Dbg("not projecting because destination is decimal degrees")
            Exit Sub
        End If

        DestPrj = pj_init_plus(projCmdLine)
        If DestPrj = 0 Then
            Logger.Msg("Could not initialize proj.dll, aborting projection" & vbCr & projCmdLine, "ShapeProject")
            Exit Sub
        End If

        projCmdLine = projInitFromFile(projectionSource)

        If InStr(projCmdLine, "proj=dd ") > 0 Then
            ConvertToRadians = True
            projCmdLine = projCmdLine.Replace("proj=dd ", "proj=latlong ")
        End If
        Logger.Dbg("projectionSource = " & projCmdLine)

        SrcPrj = pj_init_plus(projCmdLine)
        If SrcPrj = 0 Then
            Logger.Msg("Could not initialize proj.dll for output:" & vbCrLf & projCmdLine & vbCrLf & "aborting projection", "ShapeProject")
            Exit Sub
        End If

        For Each lFilename In shpFileNames
            curBaseName = FilenameNoExt(lFilename)
            curShpName = curBaseName & ".shp"
            If Len(curBaseName) = 0 Then
                'skip empty filename
            ElseIf Not shpIn.ShapeFileOpen(curShpName, CShape_IO.READWRITEFLAG.Readwrite) Then
                Logger.Dbg("ShapeProject Could not open '" & curShpName & "'")
            Else
                Logger.Dbg("ShapeProject Converting " & shpIn.getRecordCount & " shapes in " & curShpName)
                tmpSHPname = IO.Path.GetDirectoryName(curShpName) & "\projtemp.shp"
                tmpSHXname = FilenameNoExt(tmpSHPname) & ".shx"
                If IO.File.Exists(tmpSHPname) Then Kill(tmpSHPname)
                If IO.File.Exists(tmpSHXname) Then Kill(tmpSHXname)
                shpOut.CreateNewShape(tmpSHPname, shpIn.getShapeHeader.ShapeType)

                Select Case shpIn.getShapeHeader.ShapeType
                    Case CShape_IO.FILETYPEENUM.typePoint : ProjectPoints(shpIn, shpOut)
                        'TODO: typePointZ, typePointM
                    Case CShape_IO.FILETYPEENUM.typePolyline, CShape_IO.FILETYPEENUM.typePolygon, CShape_IO.FILETYPEENUM.typeMultipoint, CShape_IO.FILETYPEENUM.typePolyLineZ, CShape_IO.FILETYPEENUM.typePolygonZ, CShape_IO.FILETYPEENUM.typeMultiPointZ, CShape_IO.FILETYPEENUM.typePolyLineM, CShape_IO.FILETYPEENUM.typePolygonM, CShape_IO.FILETYPEENUM.typeMultiPointM, CShape_IO.FILETYPEENUM.typeMultiPatch
                        ProjectPolys(shpIn, shpOut)
                    Case Else
                        Logger.Msg("Unsupported shape type " & shpIn.getShapeHeader.ShapeType, "ShapeProject")
                        Exit Sub
                End Select
                shpIn.FileShutDown()
                shpOut.FileShutDown()
                shpIn = Nothing
                shpOut = Nothing
                Kill(curShpName)
                Rename(tmpSHPname, curShpName)
                Kill(curBaseName & ".shx")
                Rename(tmpSHXname, curBaseName & ".shx")
                Logger.Dbg("ShapeProject Finished with " & curShpName)
            End If
        Next

        pj_free(SrcPrj)
        pj_free(DestPrj)

        Exit Sub

ErrHand:
        If Err.Description = "latitude or longitude exceeded limits" Then
            Logger.Msg("Could not project '" & lfilename & "'" & vbCr & "Probably it was already projected", "ShapeUtil Projection")
        Else
            Logger.Msg(Err.Description & " (" & Err.Source & ", #" & Err.Number & ")", "ShapeUtil Projection")
        End If
        If SrcPrj <> 0 Then pj_free(SrcPrj)
        If DestPrj <> 0 Then pj_free(DestPrj)
        If Not shpIn Is Nothing Then
            shpIn.FileShutDown()
            shpIn = Nothing
        End If
        If Not shpOut Is Nothing Then
            shpOut.FileShutDown()
            shpOut = Nothing
        End If
        If IO.File.Exists(tmpSHPname) Then Kill(tmpSHPname)
        If IO.File.Exists(tmpSHXname) Then Kill(tmpSHXname)
    End Sub
	
	Private Function projInitFromFile(ByRef Filename As String) As String
		Dim init As String
		Dim startpos As Integer
		Dim EOLpos As Integer
        If IO.File.Exists(Filename) Then
            init = IO.File.ReadAllText(Filename)
            startpos = InStr(init, "#")
            While startpos > 0
                EOLpos = InStr(startpos + 1, init, vbLf)
                If EOLpos = 0 Then EOLpos = Len(init)
                init = Left(init, startpos - 1) & Mid(init, EOLpos + 1)
                startpos = InStr(init, "#")
            End While
            'Get rid of "proj" at the start of the file, but not "+proj=" later
            startpos = InStr(init, "proj")
            If Mid(init, startpos + 4, 1) <> "=" Then init = Left(init, startpos - 1) & Mid(init, startpos + 4)
            init = init.Replace(vbCrLf, " ")
            init = init.Replace(vbCr, " ")
            init = init.Replace(vbLf, " ")
            init = init.Replace(" end", "")
            projInitFromFile = Trim(init)
        Else
            projInitFromFile = "+proj=dd +ellps=clrk66"
        End If
	End Function
	
	Private Sub ConvertPoint(ByRef x As Double, ByRef y As Double)
		Dim Z As Double
		Dim retcode As Integer
        'Logger.Dbg "Converting " & x & " " & y
		If ConvertToRadians Then
			x = x * DegreesToRadians
			y = y * DegreesToRadians
		End If
		retcode = pj_transform(SrcPrj, DestPrj, 1, 0, x, y, Z)
		Dim msg As String
		If retcode <> 0 Then
			Select Case retcode
				Case -1 : msg = "no arguments in initialization list"
				Case -2 : msg = "no options found in 'init' file"
				Case -3 : msg = "no colon in init= string"
				Case -4 : msg = "projection not named"
				Case -5 : msg = "unknown projection id"
				Case -6 : msg = "effective eccentricity = 1."
				Case -7 : msg = "unknown unit conversion id"
				Case -8 : msg = "invalid boolean param argument"
				Case -9 : msg = "unknown elliptical parameter name"
				Case -10 : msg = "reciprocal flattening (1/f) = 0"
				Case -11 : msg = "|radius reference latitude| > 90"
				Case -12 : msg = "squared eccentricity < 0"
				Case -13 : msg = "major axis or radius = 0 or not given"
				Case -14 : msg = "latitude or longitude exceeded limits"
				Case -15 : msg = "invalid x or y"
				Case -16 : msg = "improperly formed DMS value"
				Case -17 : msg = "non-convergent inverse meridinal dist"
				Case -18 : msg = "non-convergent inverse phi2"
				Case -19 : msg = "acos/asin: |arg| >1.+1e-14"
				Case -20 : msg = "tolerance condition error"
				Case -21 : msg = "conic lat_1 = -lat_2"
				Case -22 : msg = "lat_1 >= 90"
				Case -23 : msg = "lat_1 = 0"
				Case -24 : msg = "lat_ts >= 90"
				Case -25 : msg = "no distance between control points"
				Case -26 : msg = "projection not selected to be rotated"
				Case -27 : msg = "W <= 0 or M <= 0"
				Case -28 : msg = "lsat not in 1-5 range"
				Case -29 : msg = "path not in range"
				Case -30 : msg = "h <= 0"
				Case -31 : msg = "k <= 0"
				Case -32 : msg = "lat_0 = 0 or 90 or alpha = 90"
				Case -33 : msg = "lat_1=lat_2 or lat_1=0 or lat_2=90"
				Case -34 : msg = "elliptical usage required"
				Case -35 : msg = "invalid UTM zone number"
				Case -36 : msg = "arg(s) out of range for Tcheby eval"
				Case -37 : msg = "failed to find projection to be rotated"
				Case -38 : msg = "failed to load NAD27-83 correction file"
				Case -39 : msg = "both n & m must be spec'd and > 0"
				Case -40 : msg = "n <= 0, n > 1 or not specified"
				Case -41 : msg = "lat_1 or lat_2 not specified"
				Case -42 : msg = "|lat_1| == |lat_2|"
				Case -43 : msg = "lat_0 is pi/2 from mean lat"
				Case -44 : msg = "unparseable coordinate system definition"
				Case Else : msg = "unknown error #" & retcode
			End Select
			Err.Raise(vbObjectError + 513, "pj_transform", msg)
		End If
		'DoEvents
        'Logger.Dbg "Converted  " & x & " " & y
	End Sub
	
	Private Sub ProjectPoints(ByRef shpIn As CShape_IO, ByRef shpOut As CShape_IO)
		Dim record, nRecords As Integer
        Dim lXYPoint As ShapeDefines.T_shpXYPoint
		
		nRecords = shpIn.getRecordCount
		For record = 1 To nRecords
            lXYPoint = shpIn.getXYPoint(record)
            ConvertPoint(lXYPoint.thePoint.x, lXYPoint.thePoint.y)
            shpOut.putXYPoint(0, lXYPoint)
		Next 
	End Sub
	
	'Private Sub ProjectMultiPoints(shpIn As CShape_IO, shpOut As CShape_IO)
	'  Dim record As Long, nRecords As Long
	'  Dim aXYMultiPoint As ShapeDefines.T_shpXYMultiPoint
	'  Dim point&
	'
	'  nRecords = shpIn.getRecordCount
	'  For record = 1 To nRecords
	'    aXYMultiPoint = shpIn.getXYMultiPoint(record)
	'    With aXYMultiPoint
	'      ProjectBoundingBox .Box
	'      For point = 0 To .NumPoints - 1
	'        ConvertPoint .thePoints(point).x, .thePoints(point).y
	'      Next point
	'    End With
	'    shpOut.putXYMultiPoint 0, aXYMultiPoint
	'  Next
	'End Sub
	
	'Private Sub ProjectPolylines(shpIn As CShape_IO, shpOut As CShape_IO)
	'  Dim record As Long, nRecords As Long
	'  Dim aPolyLine As ShapeDefines.T_shpPolyLine
	'  Dim part&, point&, maxpoint&
	'
	'  nRecords = shpIn.getRecordCount
	'  For record = 1 To nRecords
	'    aPolyLine = shpIn.getPolyLine(record)
	'    point = 0
	'    With aPolyLine
	'      ProjectBoundingBox .Box
	'      For part = 0 To .NumParts - 1
	'        If part < .NumParts - 1 Then
	'          maxpoint = .Parts(part + 1)
	'        Else
	'          maxpoint = .NumPoints
	'        End If
	'        While point < maxpoint
	'          ConvertPoint .thePoints(point).x, .thePoints(point).y
	'          point = point + 1
	'        Wend
	'      Next part
	'    End With
	'    shpOut.putPolyLine 0, aPolyLine
	'  Next
	'End Sub
	
	Private Sub ProjectPolys(ByRef shpIn As CShape_IO, ByRef shpOut As CShape_IO)
		Dim record, nRecords As Integer
		Dim lPoly As ShapeDefines.T_shpPoly
		Dim point, part, maxpoint As Integer
		
		nRecords = shpIn.getRecordCount
		For record = 1 To nRecords
            lPoly = shpIn.getPoly(record)
			point = 0
			With lPoly
				ProjectBoundingBox(.Box)
				For part = 0 To .NumParts - 1
					If part < .NumParts - 1 Then
						maxpoint = .Parts(part + 1)
					Else
						maxpoint = .NumPoints
					End If
					While point < maxpoint
						ConvertPoint(.thePoints(point).x, .thePoints(point).y)
						point = point + 1
					End While
				Next part
			End With
			shpOut.putPoly(0, lPoly)
		Next 
	End Sub
	
	'Have to check all four corners of old bounding box for new mins and maxes
	Private Sub ProjectBoundingBox(ByRef Box As T_BoundingBox)
		Dim newBox As T_BoundingBox
		Dim x As Double
		Dim y As Double
		
		x = Box.xMin
		y = Box.yMin
		ConvertPoint(x, y)
		newBox.xMin = x
		newBox.xMax = x
		newBox.yMin = y
		newBox.yMax = y
		
		x = Box.xMin
		y = Box.yMax
		ConvertPoint(x, y)
		If x < newBox.xMin Then newBox.xMin = x
		If x > newBox.xMax Then newBox.xMax = x
		If y < newBox.yMin Then newBox.yMin = y
		If y > newBox.yMax Then newBox.yMax = y
		
		x = Box.xMax
		y = Box.yMin
		ConvertPoint(x, y)
		If x < newBox.xMin Then newBox.xMin = x
		If x > newBox.xMax Then newBox.xMax = x
		If y < newBox.yMin Then newBox.yMin = y
		If y > newBox.yMax Then newBox.yMax = y
		
		x = Box.xMax
		y = Box.yMax
		ConvertPoint(x, y)
		If x < newBox.xMin Then newBox.xMin = x
		If x > newBox.xMax Then newBox.xMax = x
		If y < newBox.yMin Then newBox.yMin = y
		If y > newBox.yMax Then newBox.yMax = y
		
        Box = newBox
	End Sub
End Module