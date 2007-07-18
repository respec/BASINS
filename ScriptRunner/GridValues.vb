Imports atcData
Imports atcData.atcDataGroup
Imports atcUtility
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinGeoProc
Imports atcMwGisUtility

Imports Microsoft.VisualBasic
Imports System

Public Module ScriptGridValues
    Private pTestPath As String = "D:\GisData\SERDP"
    Private pStepSize As Double = 1.0 'meters
    Private pStepCount As Integer = 160 'leads to 160m wide cross section
    Private Const pFormat As String = "#,##0.000"
    Private pTemplateLayer As Integer = -1
    Private pLidarId As Integer = 0

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("Start")
        'change to the directory of the current project
        ChDriveDir(pTestPath)
        Logger.Dbg(" CurDir:" & CurDir())
        Dim lString As New Text.StringBuilder
        Dim lXSectionString As New Text.StringBuilder
        Dim lPointCollection As atcCollection
        Dim lPointCollectionX As atcCollection
        Dim lX, lY, lZMin, lZMinOffset, lXCenter, lYCenter, lZCenter, dX, dY, dH, lRatio As Double
        Dim lXCpArray() As Double = {691050.0, 705224.72, 707123.89, 709716.24, 701852.66, 696375.7, 712539.64, 690978.93}
        Dim lYCpArray() As Double = {3584061.0, 3592641.85, 3583792.35, 3587516.86, 3573066.52, 3574033.65, 3587509.2, 3580256.11}

        For lIndex As Integer = 0 To lXCpArray.GetUpperBound(0)
            lPointCollection = XY2ZLidarFromTemplate(aMapWin, lXCpArray(lIndex), lYCpArray(lIndex))
            ReportPoints(lIndex, lXCpArray(lIndex), lYCpArray(lIndex), lPointCollection, lString)
        Next

        'Dim lStepSize As Integer = 2000 'meters
        'lString.AppendLine("SamplePointsFromGridWithStep " & lStepSize)
        'For lX = 688000 To 724000 Step lStepSize
        '    For lY = 3568000 To 3604000 Step lStepSize
        '        lPointCollection = XY2Z(aMapWin, lX, lY)
        '        ReportPoints(lX, lY, lPointCollection, lString)
        '    Next lY
        'Next lX

        'mapwingeoproc.CrossSection.GetCrossSection(

        Dim lCrossSections As New MapWinGIS.Shapefile
        Dim lCrossSectionShapeName As String = "CrossSections"
        If FileExists(lCrossSectionShapeName & ".shp") Then Kill(lCrossSectionShapeName & ".shp")
        If FileExists(lCrossSectionShapeName & ".shx") Then Kill(lCrossSectionShapeName & ".shx")
        If FileExists(lCrossSectionShapeName & ".dbf") Then Kill(lCrossSectionShapeName & ".dbf")
        lCrossSections.CreateNew(lCrossSectionShapeName & ".shp", MapWinGIS.ShpfileType.SHP_POLYLINEZ)
        Dim lFieldDefinitionX As New MapWinGIS.Field
        lFieldDefinitionX.Name = "X"
        lFieldDefinitionX.Type = MapWinGIS.FieldType.DOUBLE_FIELD
        lFieldDefinitionX.Width = 16
        lFieldDefinitionX.Precision = 0
        lCrossSections.EditInsertField(lFieldDefinitionX, 0)
        Dim lFieldDefinitionY As New MapWinGIS.Field
        lFieldDefinitionY.Name = "Y"
        lFieldDefinitionY.Type = MapWinGIS.FieldType.DOUBLE_FIELD
        lFieldDefinitionY.Width = 16
        lFieldDefinitionY.Precision = 0
        lCrossSections.EditInsertField(lFieldDefinitionY, 1)
        Dim lFieldDefinitionZ As New MapWinGIS.Field
        lFieldDefinitionZ.Name = "Z"
        lFieldDefinitionZ.Type = MapWinGIS.FieldType.DOUBLE_FIELD
        lFieldDefinitionZ.Width = 16
        lFieldDefinitionZ.Precision = 5
        lCrossSections.EditInsertField(lFieldDefinitionZ, 2)
        Dim lFieldDefinitionL As New MapWinGIS.Field
        lFieldDefinitionL.Name = "L"
        lFieldDefinitionL.Type = MapWinGIS.FieldType.DOUBLE_FIELD
        lFieldDefinitionL.Width = 16
        lFieldDefinitionL.Precision = 5
        lCrossSections.EditInsertField(lFieldDefinitionL, 3)
        Dim lFieldDefinitionZOffset As New MapWinGIS.Field
        lFieldDefinitionZOffset.Name = "ZOffset"
        lFieldDefinitionZOffset.Type = MapWinGIS.FieldType.DOUBLE_FIELD
        lFieldDefinitionZOffset.Width = 16
        lFieldDefinitionZOffset.Precision = 5
        lCrossSections.EditInsertField(lFieldDefinitionZOffset, 4)
        Dim lFieldDefinitionZOrig As New MapWinGIS.Field
        lFieldDefinitionZOrig.Name = "ZOrig"
        lFieldDefinitionZOrig.Type = MapWinGIS.FieldType.DOUBLE_FIELD
        lFieldDefinitionZOrig.Width = 16
        lFieldDefinitionZOrig.Precision = 5
        lCrossSections.EditInsertField(lFieldDefinitionZOrig, 5)
        lCrossSections.StartEditingTable()
        lCrossSections.StartEditingShapes(True)

        lXSectionString.AppendLine("StepSize " & pStepSize & " StepCount " & pStepCount)
        lXSectionString.Append("Id" & vbTab & "DS km" & "Offset")
        For lPointIndex As Integer = 0 To pStepCount
            lXSectionString.Append(vbTab & lPointIndex * pStepSize)
        Next
        lXSectionString.Append(vbCrLf)

        lString.AppendLine("SamplePointsFromStreamDownstream2Upstream")
        GisUtil.MappingObject = aMapWin
        Dim lLayerIndex As Integer = GisUtil.LayerIndex("UpatoiNHDFlowline")
        Logger.Dbg("Found Upatoi:ShapeCount:" & GisUtil.NumFeatures(lLayerIndex))

        Dim lComIdFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, "COMID")
        Dim lToComIdFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, "TOCOMID")
        Dim lLengthFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, "LENGTHKM")
        Dim lLengthTotal As Double = 0
        Dim lLengthSegment As Double
        Dim lLength As Double
        Dim lCnt As Integer = 0
        Dim lCrosssectionCount As Integer = 0

        Dim lComId As Integer = 3434322 'downstream COMID for Upitoi Creek from visual inspection
        Do While lComId > 0
            Dim lFeatureIndex As Integer = GisUtil.FindFeatureIndex(lLayerIndex, lComIdFieldIndex, lComId.ToString)
            Dim lXArray() As Double = Nothing
            Dim lYArray() As Double = Nothing
            lLengthSegment = 0
            GisUtil.PointsOfLine(lLayerIndex, lFeatureIndex, lXArray, lYArray)
            For lIndex As Integer = lXArray.GetUpperBound(0) To 0 Step -1 'points are ordered upstream to downstream
                lPointCollection = XY2Z(aMapWin, lXArray(lIndex), lYArray(lIndex))
                If lIndex = lXArray.GetUpperBound(0) Then
                    lLength = 0
                Else
                    dX = lXArray(lIndex + 1) - lXArray(lIndex)
                    dY = lYArray(lIndex + 1) - lYArray(lIndex)
                    dH = Math.Sqrt((dX ^ 2) + (dY ^ 2)) 'meters
                    lLength = dH / 1000.0 'km
                    'build a crosssection for this segment from its perpendicular bisector
                    lRatio = pStepSize / dH
                    lXCenter = lXArray(lIndex) + (dX / 2)
                    lX = lXCenter + (dY * lRatio * (pStepCount / 2)) 'x offset to start of Xsection
                    lYCenter = lYArray(lIndex) + (dY / 2)
                    lY = lYCenter - (dX * lRatio * (pStepCount / 2)) 'y offset to start of Xsection

                    Dim lCrossSection As New MapWinGIS.Shape
                    Dim lPointCount As Integer = 0
                    lCrossSection.Create(MapWinGIS.ShpfileType.SHP_POLYLINEZ)
                    lZMin = 1.0E+30
                    lXSectionString.Append(lCrosssectionCount & vbTab & _
                                           DoubleToString(lLengthTotal + lLengthSegment + (lLength / 2), , pFormat, , , 5) & vbTab)
                    For lPointIndex As Integer = 0 To pStepCount
                        lPointCollectionX = XY2Z(aMapWin, lX, lY)
                        Dim lPoint As New MapWinGIS.Point
                        lPoint.x = lX
                        lPoint.y = lY
                        lPoint.Z = lPointCollectionX.ItemByIndex(lPointCollectionX.Count - 1)
                        lXSectionString.Append(vbTab & DoubleToString(lPoint.Z, , pFormat, , , 8))
                        If lPoint.Z < lZMin Then
                            lZMin = lPoint.Z
                            lZMinOffset = Math.Abs(lPointIndex - (pStepCount / 2)) * pStepSize
                        End If
                        If lPointIndex = pStepCount / 2 Then 'elevation from orig nhd flowline
                            lZCenter = lPoint.Z
                        End If
                        lCrossSection.InsertPoint(lPoint, lPointIndex)
                        lX += -(dY * lRatio)
                        lY += +(dX * lRatio)
                    Next
                    lXSectionString.AppendLine()
                    lCrossSections.EditInsertShape(lCrossSection, lCrosssectionCount)
                    lCrossSections.EditCellValue(0, lCrosssectionCount, lXCenter) ' x point of perpendicular bisector
                    lCrossSections.EditCellValue(1, lCrosssectionCount, lYCenter) ' y point of perpendicular bisector
                    lCrossSections.EditCellValue(2, lCrosssectionCount, lLengthTotal + lLengthSegment + lLength)
                    lCrossSections.EditCellValue(3, lCrosssectionCount, lZMin)
                    lCrossSections.EditCellValue(4, lCrosssectionCount, lZMinOffset)
                    lCrossSections.EditCellValue(5, lCrosssectionCount, lZCenter)
                    lCrosssectionCount += 1
                End If
                lLengthSegment += lLength
                Logger.Dbg("   CalcSegment " & lIndex & _
                             " Length " & DoubleToString(lLength, , pFormat, , , 6) & _
                             " CumSegmentLength " & DoubleToString(lLengthSegment, , pFormat, , , 6))
                ReportPoints(lCrosssectionCount, lXArray(lIndex), lYArray(lIndex), lPointCollection, lString, lLengthTotal + lLengthSegment)
            Next lIndex
            lLengthTotal += lLengthSegment
            Logger.Dbg("  ComId:" & lComId & " FeatureIndex:" & lFeatureIndex & _
                                             " Length:" & DoubleToString(lLengthSegment, , pFormat, , , 6) & _
                                             " from DBF " & GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lLengthFieldIndex) & _
                                             " LengthTotal:" & DoubleToString(lLengthTotal, , pFormat, , , 6))

            Dim lFeatureIndexUpstream As Integer = GisUtil.FindFeatureIndex(lLayerIndex, lToComIdFieldIndex, lComId.ToString)
            If lFeatureIndexUpstream >= 0 Then
                lComId = GisUtil.FieldValue(lLayerIndex, lFeatureIndexUpstream, lComIdFieldIndex)
                lCnt += 1
            Else
                lComId = 0
            End If
        Loop
        lCrossSections.StopEditingShapes(True)
        lCrossSections.StopEditingTable(True)
        lCrossSections.SaveAs(lCrossSectionShapeName & ".shp")

        Logger.Dbg("ShapeProcessedCount:" & lCnt & " Length:" & lLengthTotal)
        SaveFileString("GridSample.txt", lString.ToString)
        SaveFileString("XSection.txt", lXSectionString.ToString)
    End Sub

    Sub ReportPoints(ByVal aIndex As Integer, ByVal aX As Double, ByVal aY As Double, ByVal aPointCollection As atcCollection, ByRef aString As Text.StringBuilder, Optional ByVal aPos As Double = -1)
        If aPointCollection.Count > 1 Then
            Dim lRatio As Double = aPointCollection(0) / aPointCollection(1)
            If aPos > -1 Then
                aString.Append(DoubleToString(aPos, 5) & vbTab)
            End If
            aString.Append(aindex & vbtab & _
                           DoubleToString(aX, 14, , , , 10) & vbTab & _
                           DoubleToString(aY, 14, , , , 10) & vbTab & _
                           DoubleToString(aPointCollection(0), , pFormat, , , 6) & vbTab & _
                           aPointCollection(1) & vbTab & _
                           DoubleToString(lRatio))
            For lIndex As Integer = 3 To aPointCollection.Count
                aString.Append(vbTab & DoubleToString(aPointCollection(lIndex - 1), , pFormat, , , 10))
            Next lIndex
            aString.AppendLine()
        End If
    End Sub

    Function XY2ZLidarFromTemplate(ByRef aMapWin As IMapWin, ByVal aX As Double, ByVal aY As Double) As atcCollection
        Dim lZ As Double
        Dim lR, lC As Integer
        Dim lLidarId As Integer
        Dim lOutCollection As New atcCollection

        If pTemplateLayer < 0 Then 'what is the template layer
            For lLayerIndex As Integer = 0 To aMapWin.Layers.NumLayers - 1
                If aMapWin.Layers(lLayerIndex).LayerType = eLayerType.PolygonShapefile Then
                    If aMapWin.Layers(lLayerIndex).Name.StartsWith("insta") Then 'TODO: dont hardcode name
                        pTemplateLayer = lLayerIndex
                    End If
                End If
            Next
        End If

        'which lidar image?
        Dim lPolygonSf As MapWinGIS.Shapefile = aMapWin.Layers(pTemplateLayer).GetObject

        lPolygonSf.BeginPointInShapefile()
        'TODO: dont hard code field 1 for "tile_num"
        lLidarId = lPolygonSf.CellValue(1, (lPolygonSf.PointInShapefile(aX, aY)))
        lPolygonSf.EndPointInShapefile()

        If lLidarId <> pLidarId Then
            '  not in memory, remove old, add new
        End If

        'do the lidar image (and any others in memory
        For lLayerIndex As Integer = 0 To aMapWin.Layers.NumLayers - 1
            If aMapWin.Layers(lLayerIndex).LayerType = eLayerType.Grid Then
                Dim lGrid As MapWinGIS.Grid = aMapWin.Layers(lLayerIndex).GetGridObject
                lGrid.ProjToCell(aX, aY, lC, lR)
                lZ = lGrid.Value(lC, lR)
                If lZ > 1000 Then 'cm to m kludge
                    lZ /= 100
                End If
                If lZ > 0 Then
                    lOutCollection.Add(lLayerIndex, lZ)
                End If
            End If
        Next
        Return lOutCollection
    End Function

    Function XY2Z(ByRef aMapWin As IMapWin, ByVal aX As Double, ByVal aY As Double) As atcCollection
        Dim lZ As Double
        Dim lR, lC As Integer
        Dim lOutCollection As New atcCollection

        For lLayerIndex As Integer = 0 To aMapWin.Layers.NumLayers - 1
            If aMapWin.Layers(lLayerIndex).LayerType = eLayerType.Grid Then
                Dim lGrid As MapWinGIS.Grid = aMapWin.Layers(lLayerIndex).GetGridObject
                lGrid.ProjToCell(aX, aY, lC, lR)
                lZ = lGrid.Value(lC, lR)
                If lZ > 1000 Then 'cm to m kludge
                    lZ /= 100
                End If
                If lZ > 0 Then
                    lOutCollection.Add(lLayerIndex, lZ)
                End If
            End If
        Next
        Return lOutCollection
    End Function
End Module
