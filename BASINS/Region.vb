Imports atcUtility
Imports MapWinUtility
Imports MapWinGIS

Public Class Region
    'TODO: make this a base class, move code that depends on kind of Region (box, circle, polygon) into subclasses

    Private Shared pNationalHUC8shapefilename As String = Nothing
    Private Shared pNationalHUC8keyField As String = "CU"

    Friend pNorth As Double = GetNaN()
    Friend pSouth As Double = pNorth
    Friend pWest As Double = pNorth
    Friend pEast As Double = pNorth
    Friend pProjection As String = ""

    Friend pHUC8s As New Generic.List(Of String)
    Friend pPreferredFormat As String = "box"

    Public Sub New(ByVal aNorth As Double, _
                   ByVal aSouth As Double, _
                   ByVal aWest As Double, _
                   ByVal aEast As Double, _
                   ByVal aProjection As String)
        pNorth = aNorth
        pSouth = aSouth
        pWest = aWest
        pEast = aEast
        pProjection = aProjection
        Validate()
    End Sub

    Public Sub New(ByVal aArgs As Xml.XmlNode)
        SetXML(aArgs)
    End Sub

    Private Sub SetXML(ByVal aXML As Xml.XmlNode)
        Dim lArg As Xml.XmlNode = aXML.FirstChild
        While Not lArg Is Nothing
            If lArg.Name.ToLower.Equals("arg") Then 'old style
                Dim lName As String = lArg.Attributes.GetNamedItem("name").Value
                If lName.ToLower.Equals("region") Then
                    SetXML(lArg)
                Else
                    SetPrivate(lName, lArg.InnerText)
                End If
            Else
                If lArg.Name.ToLower.Equals("region") Then
                    SetXML(lArg)
                Else
                    SetPrivate(lArg.Name, lArg.InnerText)
                End If
            End If
            lArg = lArg.NextSibling
        End While
        Validate()
    End Sub

    Private Sub SetPrivate(ByVal aPart As String, ByVal aNewValue As String)
        Select Case aPart.ToLower
            Case "northbc", "top" : If IsNumeric(aNewValue) Then pNorth = CDbl(aNewValue)
            Case "southbc", "bottom" : If IsNumeric(aNewValue) Then pSouth = CDbl(aNewValue)
            Case "westbc", "left" : If IsNumeric(aNewValue) Then pWest = CDbl(aNewValue)
            Case "eastbc", "right" : If IsNumeric(aNewValue) Then pEast = CDbl(aNewValue)
            Case "huc8" : AddHuc8(aNewValue)
            Case "huc12" : AddHuc8(SafeSubstring(aNewValue, 0, 8))
            Case "preferredformat" : pPreferredFormat = aNewValue
            Case "projection", "boxprojection" : pProjection = aNewValue
        End Select
    End Sub

    Private Sub AddHuc8(ByVal aHUC8 As String)
        If aHUC8 IsNot Nothing AndAlso aHUC8.Length = 8 AndAlso IsNumeric(aHUC8) AndAlso Not pHUC8s.Contains(aHUC8) Then
            pHUC8s.Add(aHUC8)
        End If
    End Sub

    Public Overridable Function XML() As String
        Dim lHUC8XML As String = ""
        For Each lhuc8 As String In pHUC8s
            lHUC8XML &= "<HUC8>" & lhuc8 & "</HUC8>" & vbCrLf
        Next
        Return "<region>" & vbCrLf _
             & "<northbc>" & pNorth & "</northbc>" & vbCrLf _
             & "<southbc>" & pSouth & "</southbc>" & vbCrLf _
             & "<eastbc>" & pEast & "</eastbc>" & vbCrLf _
             & "<westbc>" & pWest & "</westbc>" & vbCrLf _
             & lHUC8XML _
             & "<preferredformat>" & pPreferredFormat & "</preferredformat>" & vbCrLf _
             & "<projection>" & pProjection & "</projection>" & vbCrLf _
             & "</region>" & vbCrLf
    End Function

    Public Overridable Sub Validate()
        If pHUC8s.Count = 0 Then
            If Double.IsNaN(pNorth) Then
                Throw New ApplicationException("North not specified")
            ElseIf Double.IsNaN(pSouth) Then
                Throw New ApplicationException("South not specified")
            ElseIf Double.IsNaN(pWest) Then
                Throw New ApplicationException("West not specified")
            ElseIf Double.IsNaN(pEast) Then
                Throw New ApplicationException("East not specified")
            ElseIf pProjection.Length = 0 Then
                Throw New ApplicationException("Projection not specified")
            End If
        End If
    End Sub

    ''' <summary>
    ''' Overlay this region on a shape file and returns a value from the
    ''' shape table for each shape that overlaps this region.
    ''' Example: return the list of county FIPS codes that overlap the region.
    ''' </summary>
    ''' <param name="aSelectFromShapeFilename">Select shapes from this layer</param>
    ''' <param name="aKeyField">Name or index of field in shape DBF to get values from</param>
    ''' <param name="aSelectFromShapeProjection">Projection of aSelectFromShapeFilename, Nothing=get projection from the layer</param>
    ''' <returns>List of values from aKeyField of shapes overlapping this Region</returns>
    Public Overridable Function GetKeysOfOverlappingShapes(ByVal aSelectFromShapeFilename As String, _
                                                           ByVal aKeyField As String, _
                                                           ByVal aSelectFromShapeProjection As String) As Generic.List(Of String)
        Dim lSelectFromLayer As New MapWinGIS.Shapefile
        If Not lSelectFromLayer.Open(aSelectFromShapeFilename) Then
            Throw New ApplicationException("Could not open layer for selection '" & aSelectFromShapeFilename & "'")
        End If

        Dim lSelectedShapesFilename As String = GetTemporaryFileName("overlap", "shp")
        Dim lKeyField As Integer = -1

        If IsNumeric(aKeyField) Then
            lKeyField = CInt(aKeyField)
        Else 'find named field
            Dim lLowerFieldName As String = aKeyField.ToLower
            For iField As Integer = 0 To lSelectFromLayer.NumFields - 1
                Dim lField As MapWinGIS.Field = lSelectFromLayer.Field(iField)
                If lField.Name.ToLower.Equals(lLowerFieldName) Then
                    lKeyField = iField
                    Exit For
                End If
            Next
        End If

        If lKeyField < 0 OrElse lKeyField >= lSelectFromLayer.NumFields Then
            Throw New ApplicationException("Could not find field '" & aKeyField & "'")
        End If
        Return GetKeysOfOverlappingShapes(lSelectFromLayer, lKeyField, aSelectFromShapeProjection)
    End Function

    ''' <summary>
    ''' Overlay this region on a shape file and returns a value from the
    ''' shape table for each shape that overlaps this region.
    ''' Example: return the list of county FIPS codes that overlap the region.
    ''' </summary>
    ''' <param name="aSelectFromLayer">Select shapes from this layer</param>
    ''' <param name="aKeyField">Zero-indexed field in aSelectFromLayer</param>
    ''' <param name="aSelectFromShapeProjection">Projection of aSelectFromLayer, Nothing=get projection from aSelectFromLayer</param>
    ''' <returns>List of values from aKeyField of shapes overlapping this Region</returns>
    Friend Overridable Function GetKeysOfOverlappingShapes(ByVal aSelectFromLayer As MapWinGIS.Shapefile, _
                                                           ByVal aKeyField As Integer, _
                                                           ByVal aSelectFromShapeProjection As String) As Generic.List(Of String)
        Dim lKeys As New Generic.List(Of String)
        Dim lLastShape As Integer = aSelectFromLayer.NumShapes - 1
        Dim lLastPoint As Integer
        Dim lShape As MapWinGIS.Shape
        Dim lPoint As MapWinGIS.Point
        Dim lYMax As Double = pNorth
        Dim lYMin As Double = pSouth
        Dim lXMax As Double = pEast
        Dim lXMin As Double = pWest
        Dim lSwap As Double
        If aSelectFromShapeProjection Is Nothing OrElse aSelectFromShapeProjection.Length = 0 Then
            aSelectFromShapeProjection = aSelectFromLayer.Projection
        End If

        MapWinGeoProc.SpatialReference.ProjectPoint(lXMin, lYMax, pProjection, aSelectFromShapeProjection)
        MapWinGeoProc.SpatialReference.ProjectPoint(lXMax, lYMin, pProjection, aSelectFromShapeProjection)

        If lYMax < lYMin Then lSwap = lYMax : lYMax = lYMin : lYMin = lSwap
        If lXMax < lXMin Then lSwap = lXMax : lXMax = lXMin : lXMin = lSwap

        For iShape As Integer = 0 To lLastShape
            lShape = aSelectFromLayer.Shape(iShape)
            lLastPoint = lShape.numPoints - 1

            If lLastPoint >= 0 Then
                lPoint = lShape.Point(0)
                Dim lShapeYMax As Double = lPoint.y
                Dim lShapeYMin As Double = lPoint.y
                Dim lShapeXMax As Double = lPoint.x
                Dim lShapeXMin As Double = lPoint.x

                For iPoint As Integer = 0 To lLastPoint
                    lPoint = lShape.Point(iPoint)
                    If lPoint.x >= lXMin AndAlso _
                       lPoint.x <= lXMax AndAlso _
                       lPoint.y >= lYMin AndAlso _
                       lPoint.y <= lYMax Then
                        lKeys.Add(aSelectFromLayer.CellValue(aKeyField, iShape))
                        GoTo NextShape
                    Else
                        If lPoint.x > lShapeXMax Then
                            lShapeXMax = lPoint.x
                        ElseIf lPoint.x < lShapeXMin Then
                            lShapeXMin = lPoint.x
                        End If

                        If lPoint.y > lShapeYMax Then
                            lShapeYMax = lPoint.y
                        ElseIf lPoint.y < lShapeYMin Then
                            lShapeYMin = lPoint.y
                        End If

                    End If
                Next
                'No point was inside box
                If lShapeXMax >= lXMin AndAlso _
                   lShapeXMin <= lXMax AndAlso _
                   lShapeYMax >= lYMin AndAlso _
                   lShapeYMin <= lYMax Then
                    'Bounding box overlaps
                    'TODO: actual line/polygon intersection test
                    lKeys.Add(aSelectFromLayer.CellValue(aKeyField, iShape))
                End If
            End If
NextShape:
        Next

        ''TODO: would be more efficient to have a new version of SelectWithPolygon that does not write a new shape file
        'If MapWinGeoProc.SpatialOperations.SelectWithPolygon(aSelectFromShapeFilename, aSelectionPolygon, lSelectedShapesFilename) Then

        '    Dim lShapesSelected As New MapWinGIS.Shapefile
        '    lShapesSelected.Open(lSelectedShapesFilename)

        '    If IsNumeric(aKeyField) Then
        '        lKeyField = CInt(aKeyField)
        '    Else 'find named field
        '        Dim lLowerFieldName As String = aKeyField.ToLower
        '        For iField As Integer = 0 To lShapesSelected.NumFields - 1
        '            Dim lField As MapWinGIS.Field = lShapesSelected.Field(iField)
        '            If lField.Name.ToLower.Equals(lLowerFieldName) Then
        '                lKeyField = iField
        '                Exit For
        '            End If
        '        Next
        '    End If

        '    If lKeyField >= 0 AndAlso lKeyField < lShapesSelected.NumFields Then
        '        For iShape As Integer = 0 To lShapesSelected.NumShapes - 1
        '            lKeys.Add(lShapesSelected.CellValue(lKeyField, iShape))
        '        Next
        '    Else
        '        Throw New ApplicationException("Could not find field '" & aKeyField & "'")
        '    End If
        '    lShapesSelected.Close()
        '    lShapesSelected = Nothing
        '    MapWinGeoProc.DataManagement.DeleteShapefile(lSelectedShapesFilename)
        'Else
        '    Throw New ApplicationException("Could not SelectWithPolygon: " & MapWinGeoProc.Error.GetLastErrorMsg)
        'End If
        Return lKeys
    End Function

    Public Property PreferredFormat() As String
        Get
            Return pPreferredFormat
        End Get
        Set(ByVal newValue As String)
            pPreferredFormat = newValue
        End Set
    End Property

    Public Shared Property NationalHUC8shapefilename() As String
        Get
            If pNationalHUC8shapefilename Is Nothing Then
                pNationalHUC8shapefilename = atcUtility.FindFile(Nothing, "\basins\data\national\huc250d3.shp")
            End If
            Return pNationalHUC8shapefilename
        End Get
        Set(ByVal value As String)
            pNationalHUC8shapefilename = value
        End Set
    End Property

    Public Shared Property NationalHUC8keyField() As String
        Get
            Return pNationalHUC8keyField
        End Get
        Set(ByVal value As String)
            pNationalHUC8keyField = value
        End Set
    End Property

    Public Overridable Property HUC8s() As Generic.List(Of String)
        Get
            Try
                If pHUC8s.Count = 0 Then
                    Dim lLayerFilename As String = NationalHUC8shapefilename
                    If FileExists(lLayerFilename) Then
                        Logger.Status("Determining HUCs in region", True)
                        pHUC8s = GetKeysOfOverlappingShapes(lLayerFilename, NationalHUC8keyField, Nothing) ', "+proj=aea +lat_1=29.5 +lat_2=45.5 +lat_0=23 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs")
                        Logger.Status("Found " & pHUC8s.Count & " hucs", True)
                    Else
                        Logger.Dbg("HUC-8 layer not found for getting HUC-8s of region.")
                    End If
                End If
            Catch e As Exception
                Logger.Dbg("Exception getting HUC-8s of region: " & e.Message)
            End Try
            Return pHUC8s
        End Get
        Set(ByVal newValue As Generic.List(Of String))
            pHUC8s = newValue
        End Set
    End Property

    Public Overridable Function ClipGrid(ByVal aGridFilename As String, ByVal aClippedFilename As String, ByVal aGridProjection As String) As Boolean
retry:
        'TODO: 
        If MapWinGeoProc.SpatialOperations.ClipGridWithPolygon(aGridFilename, Me.ToShape(aGridProjection), aClippedFilename) Then
            Return True
        Else
            Dim lLastErrorMsg As String = MapWinGeoProc.Error.GetLastErrorMsg
            If lLastErrorMsg.ToLower.Contains("no error") Then GoTo retry
            Throw New ApplicationException("ClipGridWithPolygon:" & lLastErrorMsg)
            Return False
        End If
    End Function


    ''' <summary>
    ''' Copies all fields from the inputSF .dbf table to the resultSF table.
    ''' </summary>
    ''' <param name="inputSF">The shapefile with fields to be copied.</param>
    ''' <param name="resultSF">The result shapefile that will inherit the fields.</param>
    ''' <returns>False if an error was encountered, true otherwise.</returns>
    Private Function CopyFields(ByRef inputSF As MapWinGIS.Shapefile, ByRef resultSF As MapWinGIS.Shapefile) As Boolean
        Dim fieldIndex As Integer = 0
        Dim numFields As Integer = inputSF.NumFields
        For i As Integer = 0 To numFields - 1

            Dim field As MapWinGIS.Field = New MapWinGIS.Field() 'MapWinGIS would rather call this Field sometimes, other times FieldClass
            field.Name = inputSF.Field(i).Name
            field.Type = inputSF.Field(i).Type
            If (field.Type = MapWinGIS.FieldType.STRING_FIELD) Then
                field.Width = inputSF.Field(i).Width
            ElseIf (field.Type = MapWinGIS.FieldType.DOUBLE_FIELD) Then
                field.Precision = inputSF.Field(i).Precision
            End If
            fieldIndex = resultSF.NumFields

            If Not resultSF.EditInsertField(field, fieldIndex, Nothing) Then
                Logger.Dbg("Problem inserting field into result DBF table: " + resultSF.ErrorMsg(resultSF.LastErrorCode))
                Return False
            End If
        Next
        Return True
    End Function

    Private Sub CopyRecord(ByVal aFromShapefile As MapWinGIS.Shapefile, ByVal aFromIndex As Integer, _
                           ByVal aToShapefile As MapWinGIS.Shapefile, ByVal aToIndex As Integer)
        Dim lLastField As Integer = aFromShapefile.NumFields - 1
        For lFieldIndex As Integer = 0 To lLastField
            If Not (aToShapefile.EditCellValue(lFieldIndex, aToIndex, aFromShapefile.CellValue(lFieldIndex, aFromIndex))) Then
                Throw New ApplicationException("Problem inserting value into DBF table: " + aToShapefile.ErrorMsg(aToShapefile.LastErrorCode))
            End If
        Next
    End Sub

    Public Overridable Function SelectShapes(ByVal aShapeFilename As String, ByVal aClippedFilename As String, ByVal aShapeProjection As String) As Boolean

        'MapWinGeoProc.SpatialOperations.SelectWithPolygon(aShapeFilename, Me.ToShape(aShapeProjection), aClippedFilename)
        Dim lLastShape As Integer
        Dim lPointIndex As Integer
        Dim lLastPoint As Integer
        Dim lClippedIndex As Integer = 0
        Dim lBoxShape As MapWinGIS.Shape = Me.ToShape(aShapeProjection)
        Dim lxMin As Double = lBoxShape.Point(0).x
        Dim lxMax As Double = lxMin
        Dim lyMin As Double = lBoxShape.Point(0).y
        Dim lyMax As Double = lyMin
        For lPointIndex = 1 To 3
            With lBoxShape.Point(lPointIndex)
                If .x < lxMin Then lxMin = .x
                If .x > lxMax Then lxMax = .x
                If .y < lyMin Then lyMin = .y
                If .y > lyMax Then lyMax = .y
            End With
        Next

        Dim lSelectFromLayer As New MapWinGIS.Shapefile
        If lSelectFromLayer.Open(aShapeFilename) Then
            Dim lClippedLayer As New MapWinGIS.Shapefile
            Dim lNumFields As Integer = lSelectFromLayer.NumFields
            If lClippedLayer.CreateNew(aClippedFilename, lSelectFromLayer.ShapefileType) Then
                lClippedLayer.StartEditingShapes()
                lClippedLayer.StartEditingTable()
                CopyFields(lSelectFromLayer, lClippedLayer)
                Dim lShape As MapWinGIS.Shape
                Dim lSelect As Boolean = False
                lLastShape = lSelectFromLayer.NumShapes - 1
                For lFromIndex As Integer = 0 To lLastShape
                    lSelect = False
                    lShape = lSelectFromLayer.Shape(lFromIndex)
                    lLastPoint = lShape.numPoints - 1

                    Select Case lShape.ShapeType
                        Case MapWinGIS.ShpfileType.SHP_POLYGON, MapWinGIS.ShpfileType.SHP_POLYGONM, MapWinGIS.ShpfileType.SHP_POLYGONZ, _
                             MapWinGIS.ShpfileType.SHP_POLYLINE, MapWinGIS.ShpfileType.SHP_POLYLINEM, MapWinGIS.ShpfileType.SHP_POLYLINEZ
                            With lShape.Extents
                                If .xMin > lxMax OrElse .xMax < lxMin OrElse _
                                   .yMin > lyMax OrElse .yMax < lyMin Then
                                    GoTo SkipShape 'Bounding boxes do not overlap, safe to skip this shape
                                Else 'Bounding boxes overlap, go ahead and include it even though
                                    ' this will get some shapes that do not quite overlap
                                    lSelect = True
                                End If
                                'If .xMin < lxMin AndAlso .xMax > lxMax AndAlso _
                                '   .yMin < lyMin AndAlso .yMax > lyMax Then
                                '    lSelect = True 'Completely encompasses
                                'End If
                            End With
                    End Select

                    If Not lSelect Then
                        ' A point inside the bounding box is a sure sign that this shape overlaps the region
                        For lPointIndex = 0 To lLastPoint
                            With lShape.Point(lPointIndex)
                                If .x >= lxMin AndAlso .x <= lxMax AndAlso .y >= lyMin AndAlso .y <= lyMax Then
                                    lSelect = True
                                    Exit For
                                End If
                            End With
                        Next
                    End If

                    If lSelect Then 'add line to result file
                        If Not (lClippedLayer.EditInsertShape(lShape, lClippedIndex)) Then
                            Logger.Msg(lClippedLayer.ErrorMsg(lClippedLayer.LastErrorCode) & vbCrLf & aClippedFilename, "Problem inserting shape into result file")
                            Return False
                        End If
                        CopyRecord(lSelectFromLayer, lFromIndex, lClippedLayer, lClippedIndex)
                        lClippedIndex += 1
                    End If
SkipShape:
                Next
                lClippedLayer.StopEditingShapes()
                lClippedLayer.StopEditingTable()
                Logger.Dbg("Selected " & lClippedLayer.NumShapes & "=" & lClippedIndex & " of " & lSelectFromLayer.NumShapes & " from '" & aShapeFilename & "' into '" & aClippedFilename & "'.")
                lClippedLayer.Close()
            End If
            lSelectFromLayer.Close()
        End If


        If FileExists(aClippedFilename) Then
            If FileLen(aClippedFilename) = 100 Then ' empty shape file has header 100 bytes long
                Logger.Dbg("Removing empty clipped shape file '" & aClippedFilename & "'")
                atcUtility.TryDeleteShapefile(aClippedFilename)
            Else
                Dim lMetadata As Metadata
                Dim lMetadataFilename As String = aShapeFilename & ".xml"
                lMetadata = New Metadata(lMetadataFilename)
                lMetadata.AddProcessStep("Clipped to '" & Me.ToString & "'")

                Dim lWest As Double, lEast As Double, lNorth As Double, lSouth As Double
                GetBounds(lNorth, lSouth, lWest, lEast, SpatialOperations.GeographicProjection)
                lMetadata.SetBoundingBox(lWest, lEast, lNorth, lSouth)

                atcUtility.SaveFileString(aClippedFilename & ".xml", lMetadata.ToString)
            End If
            Return True
        Else
            Logger.Dbg("No shape file clipped from '" & aShapeFilename & "' to '" & aClippedFilename & "'")
            Return False
        End If
    End Function

    <CLSCompliant(False)> _
    Public Overridable Function ToShape(ByVal aNewProjection As String) As MapWinGIS.Shape
        Dim lBox As New MapWinGIS.Shape
        Dim lPoint As MapWinGIS.Point
        Dim lNeedProjection As Boolean = (Not String.IsNullOrEmpty(aNewProjection)) AndAlso Not SpatialOperations.SameProjection(aNewProjection, pProjection)
        lBox.Create(MapWinGIS.ShpfileType.SHP_POLYGON)
        lPoint = New MapWinGIS.Point
        lPoint.x = pWest
        lPoint.y = pNorth
        If lNeedProjection Then MapWinGeoProc.SpatialReference.ProjectPoint(lPoint.x, lPoint.y, pProjection, aNewProjection)
        lBox.InsertPoint(lPoint, 0)

        lPoint = New MapWinGIS.Point
        lPoint.x = pEast
        lPoint.y = pNorth
        If lNeedProjection Then MapWinGeoProc.SpatialReference.ProjectPoint(lPoint.x, lPoint.y, pProjection, aNewProjection)
        lBox.InsertPoint(lPoint, 1)

        lPoint = New MapWinGIS.Point
        lPoint.x = pEast
        lPoint.y = pSouth
        If lNeedProjection Then MapWinGeoProc.SpatialReference.ProjectPoint(lPoint.x, lPoint.y, pProjection, aNewProjection)
        lBox.InsertPoint(lPoint, 2)

        lPoint = New MapWinGIS.Point
        lPoint.x = pWest
        lPoint.y = pSouth
        If lNeedProjection Then MapWinGeoProc.SpatialReference.ProjectPoint(lPoint.x, lPoint.y, pProjection, aNewProjection)
        lBox.InsertPoint(lPoint, 3)

        Return lBox
    End Function

    Public Overridable Function GetProjected(ByVal aNewProjection As String) As Region
        Dim lNorth As Double
        Dim lSouth As Double
        Dim lEast As Double
        Dim lWest As Double
        GetBounds(lNorth, lSouth, lWest, lEast, aNewProjection)
        Dim lRegion As New Region(lNorth, lSouth, lWest, lEast, aNewProjection)
        lRegion.HUC8s = pHUC8s
        lRegion.PreferredFormat = pPreferredFormat
        Return lRegion
    End Function

    Public Overridable Sub GetBounds(ByRef aNorth As Double, _
                                     ByRef aSouth As Double, _
                                     ByRef aWest As Double, _
                                     ByRef aEast As Double, _
                            Optional ByVal aNewProjection As String = "")
        If Double.IsNaN(pNorth) OrElse Double.IsNaN(pSouth) OrElse Double.IsNaN(pWest) OrElse Double.IsNaN(pEast) Then
            BoundsFromHuc8s()
        End If

        If aNewProjection.Length > 0 AndAlso Not aNewProjection.Equals(pProjection) Then
            Dim xNW As Double = pWest, yNW As Double = pNorth
            Dim xNE As Double = pEast, yNE As Double = pNorth
            Dim xSW As Double = pWest, ySW As Double = pSouth
            Dim xSE As Double = pEast, ySE As Double = pSouth

            MapWinGeoProc.SpatialReference.ProjectPoint(xNW, yNW, pProjection, aNewProjection)
            MapWinGeoProc.SpatialReference.ProjectPoint(xNE, yNE, pProjection, aNewProjection)
            MapWinGeoProc.SpatialReference.ProjectPoint(xSW, ySW, pProjection, aNewProjection)
            MapWinGeoProc.SpatialReference.ProjectPoint(xSE, ySE, pProjection, aNewProjection)

            If yNW > yNE Then aNorth = yNW Else aNorth = yNE
            If ySW < ySE Then aSouth = ySW Else aSouth = ySE

            If xNE > xSE Then aEast = xNE Else aEast = xSE
            If xSW < xNW Then aWest = xSW Else aWest = xNW
        Else
            aNorth = pNorth
            aSouth = pSouth
            aWest = pWest
            aEast = pEast
        End If
    End Sub

    Private Sub BoundsFromHuc8s()
        If pHUC8s.Count > 0 Then
            Dim lHUC8sFilename As String = NationalHUC8shapefilename
            If FileExists(lHUC8sFilename) Then
                Dim lSelectFromLayer As New MapWinGIS.Shapefile
                If lSelectFromLayer.Open(lHUC8sFilename) Then
                    Dim lSearchHUC As New atcTableDBF
                    If lSearchHUC.OpenFile(IO.Path.ChangeExtension(lHUC8sFilename, "dbf")) Then
                        Dim lKeyField As Integer = lSearchHUC.FieldNumber(NationalHUC8keyField)
                        If lKeyField > 0 Then
                            pNorth = GetNaN()
                            pSouth = pNorth
                            pWest = pNorth
                            pEast = pNorth
                            pProjection = lSelectFromLayer.Projection
                            For Each lHUC8 As String In pHUC8s
                                If lSearchHUC.FindFirst(lKeyField, lHUC8) Then
                                    With lSelectFromLayer.QuickExtents(lSearchHUC.CurrentRecord - 1)
                                        If Double.IsNaN(pNorth) OrElse .yMax > pNorth Then pNorth = .yMax
                                        If Double.IsNaN(pSouth) OrElse .yMin < pSouth Then pSouth = .yMin
                                        If Double.IsNaN(pWest) OrElse .xMin > pWest Then pWest = .xMin
                                        If Double.IsNaN(pEast) OrElse .xMax > pEast Then pEast = .xMax
                                    End With
                                End If
                            Next
                        End If
                        lSearchHUC.Clear()
                    End If
                    lSelectFromLayer.Close()
                End If
            End If
        End If
    End Sub

    Public Overrides Function ToString() As String
        Return "Region Box North " & pNorth & " South " & pSouth & " West " & pWest & " East " & pEast
    End Function
End Class
