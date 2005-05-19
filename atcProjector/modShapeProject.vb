Option Strict Off
Option Explicit On
Module modShapeProject
	
	' proj.dll declarations
	Private Declare Function pj_init_plus Lib "projATC.dll" (ByVal init As String) As Integer
	Private Declare Sub pj_free Lib "projATC.dll" (ByVal pointer As Integer)
	Private Declare Function pj_transform Lib "projATC.dll" (ByVal SrcPrj As Integer, ByVal DestPrj As Integer, ByVal nPoints As Integer, ByVal offset As Integer, ByRef x As Double, ByRef y As Double, ByRef Z As Double) As Integer
	
    Private SrcPrj As Integer  'Pointer to source projection structure in proj.dll
	Private DestPrj As Integer 'Pointer to destination projection structure in proj.dll

    Private Const DegreesToRadians As Double = 0.01745329252
    Private ConvertToRadians As Boolean 'True if we need to convert to radians

    'projectionSource - current projection (filename or string)
    'projectionDest   - target projection (filename or string)
    'shpFile          - shape file to project from projectionSource to projectionDest
    Public Sub ProjectShapefile(ByRef projectionSource As String, ByRef projectionDest As String, ByRef shpFile As MapWinGIS.Shapefile)
        'Dim shpIn As New CShape_IO
        'Dim shpOut As New CShape_IO
        Dim firstInput As Integer
        Dim lastInput As Integer
        Dim curInput As Integer
        Dim projCmdLine As String
        Dim findPos As Integer

        Try
            projCmdLine = GetProjInitString(projectionDest)
            LogDbg("projectionDest = " & projCmdLine)
            If InStr(projCmdLine, "proj=dd ") > 0 Or InStr(projCmdLine, "proj=latlong ") > 0 Then
                LogDbg("not projecting because destination is decimal degrees")
                Exit Sub
            End If

            DestPrj = pj_init_plus(projCmdLine)
            'If DestPrj = 0 Then
            '    gLogger.LogMsg("Could not initialize proj.dll, aborting projection" & vbCr & projCmdLine, "ShapeProject")
            '    Exit Sub
            'End If

            projCmdLine = GetProjInitString(projectionSource)

            If InStr(projCmdLine, "proj=dd ") > 0 Then
                ConvertToRadians = True
                projCmdLine = ReplaceString(projCmdLine, "proj=dd ", "proj=latlong ")
            Else
                ConvertToRadians = False
            End If
            LogDbg("projectionSource = " & projCmdLine)

            SrcPrj = pj_init_plus(projCmdLine)
            If SrcPrj = 0 Then
                LogMsg("Could not initialize proj.dll for output:" & vbCrLf & projCmdLine & vbCrLf & "aborting projection", "ShapeProject")
                Exit Sub
            End If

            LogDbg("ShapeProject Converting " & shpFile.NumShapes & " shapes in " & shpFile.Filename)
            If Not shpFile.StartEditingShapes(False) Then
                'Could not start editing shapes
            Else
                Dim iShape As Integer
                Dim iPoint As Integer
                For iShape = 0 To shpFile.NumShapes - 1
                    With shpFile.Shape(iShape)
                        For iPoint = 0 To .numPoints - 1
                            ProjectPoint(.Point(iPoint).x, .Point(iPoint).y)
                        Next
                    End With
                Next
                shpFile.StopEditingShapes(True)
            End If
            LogDbg("ShapeProject Finished with " & shpFile.Filename)

        Catch e As Exception
            If e.Message = "latitude or longitude exceeded limits" Then
                LogMsg("Could not project '" & shpFile.Filename & "'" & vbCr & "Perhaps it was already projected?", "Projection")
            Else
                LogMsg(e.Message, "Projection")
            End If
        End Try
        If SrcPrj <> 0 Then pj_free(SrcPrj) : SrcPrj = 0
        If DestPrj <> 0 Then pj_free(DestPrj) : DestPrj = 0
    End Sub

    Private Function GetProjInitString(ByRef Filename As String) As String
        Dim init As String
        Dim startpos As Integer
        Dim EOLpos As Integer

        If Filename.IndexOf("+proj=") > 0 Then 'We were given a projection string
            init = Filename
        ElseIf FileExists(Filename) Then       'We were given a file name, read the file
            init = WholeFileString(Filename)
        Else                                   'Default to decimal degrees, Clark66 ellipsoid
            init = "+proj=dd +ellps=clrk66"
        End If

        'Remove commented-out lines
        startpos = InStr(init, "#")
        While startpos > 0
            EOLpos = InStr(startpos + 1, init, vbLf)
            If EOLpos = 0 Then EOLpos = Len(init)
            init = Left(init, startpos - 1) & Mid(init, EOLpos + 1)
            startpos = InStr(init, "#")
        End While

        'Get rid of "proj" at the start of the file, but not "+proj=" later
        startpos = init.IndexOf("proj")
        If startpos > -1 And init.Chars(startpos + 4) <> "=" Then init.Remove(startpos, 4)

        init = ReplaceString(init, vbCrLf, " ")
        init = ReplaceString(init, vbCr, " ")
        init = ReplaceString(init, vbLf, " ")
        init = ReplaceString(init, " end", "")

        Return init.Trim
    End Function

    Private Sub ProjectPoint(ByRef x As Double, ByRef y As Double)
        Dim Z As Double
        Dim retcode As Integer
        'gLogger.Log "Converting " & x & " " & y
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
        'gLogger.Log "Converted  " & x & " " & y
    End Sub

End Module