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
    Private pStepSize As Integer = 10 'meters
    Private pStepCount As Integer = 10 'leads to 100m wide cross section

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("Start")
        'change to the directory of the current project
        ChDriveDir(pTestPath)
        Logger.Dbg(" CurDir:" & CurDir())
        Dim lString As New Text.StringBuilder
        Dim lXSectionString As New Text.StringBuilder
        Dim lPointCollection As atcCollection
        Dim lPointCollectionX As atcCollection
        Dim lX, lY, lZMin, lXCenter, lYCenter, dX, dY, dH, lRatio As Double

        'Dim lStepSize As Integer = 2000 'meters
        'lString.AppendLine("SamplePointsFromGridWithStep " & lStepSize)
        'For lX = 688000 To 724000 Step lStepSize
        '    For lY = 3568000 To 3604000 Step lStepSize
        '        lPointCollection = XY2Z(aMapWin, lX, lY)
        '        ReportPoints(lX, lY, lPointCollection, lString)
        '    Next lY
        'Next lX

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
        lCrossSections.StartEditingTable()
        lCrossSections.StartEditingShapes(True)
        lXSectionString.AppendLine("StepSize " & pStepSize & " StepCount " & pStepCount)

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
                    lXSectionString.Append(lLengthTotal + lLengthSegment + (lLength / 2))
                    For lPointIndex As Integer = 0 To pStepCount
                        lPointCollectionX = XY2Z(aMapWin, lX, lY)
                        Dim lPoint As New MapWinGIS.Point
                        lPoint.x = lX
                        lPoint.y = lY
                        lPoint.Z = lPointCollectionX.ItemByIndex(0) / 100.0
                        lXSectionString.Append(vbTab & lPoint.Z)
                        If lPoint.Z < lZMin Then lZMin = lPoint.Z
                        lCrossSection.InsertPoint(lPoint, lPointIndex)
                        lX += -(dY * lRatio)
                        lY += +(dX * lRatio)
                    Next
                    lXSectionString.AppendLine()
                    lCrossSections.EditInsertShape(lCrossSection, lCrosssectionCount)
                    lCrossSections.EditCellValue(0, lCrosssectionCount, lXCenter) ' x point of perpendicular bisector
                    lCrossSections.EditCellValue(1, lCrosssectionCount, lYCenter) ' y point of perpendicular bisector
                    lCrossSections.EditCellValue(2, lCrosssectionCount, lZMin)
                    lCrosssectionCount += 1
                End If
                lLengthSegment += lLength
                Logger.Dbg("   CalcSegment " & lIndex & _
                             " Length " & DoubleToString(lLength, , "#,##0.###", , , 6) & _
                             " CumSegmentLength " & DoubleToString(lLengthSegment, , "#,##0.###", , , 6))
                ReportPoints(lXArray(lIndex), lYArray(lIndex), lPointCollection, lString, lLengthTotal + lLengthSegment)
            Next lIndex
            lLengthTotal += lLengthSegment
            Logger.Dbg("  ComId:" & lComId & " FeatureIndex:" & lFeatureIndex & _
                                             " Length:" & DoubleToString(lLengthSegment, , "#,##0.###", , , 6) & _
                                             " from DBF " & GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lLengthFieldIndex) & _
                                             " LengthTotal:" & DoubleToString(lLengthTotal, , "#,##0.###", , , 6))

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

    Sub ReportPoints(ByVal aX As Double, ByVal aY As Double, ByVal aPointCollection As atcCollection, ByRef aString As Text.StringBuilder, Optional ByVal aPos As Double = -1)
        If aPointCollection.Count > 1 Then
            Dim lRatio As Double = aPointCollection(0) / aPointCollection(1)
            If aPos > -1 Then
                aString.Append(DoubleToString(aPos, 5) & vbTab)
            End If
            aString.Append(DoubleToString(aX, 14, , , , 10) & vbTab & _
                           DoubleToString(aY, 14, , , , 10) & vbTab & _
                           aPointCollection(0) & vbTab & _
                           aPointCollection(1) & vbTab & _
                           DoubleToString(lRatio))
            For lIndex As Integer = 3 To aPointCollection.Count
                aString.Append(vbTab & aPointCollection(lIndex - 1) & vbTab & "???")
            Next lIndex
            aString.AppendLine()
        End If
    End Sub

    Function XY2Z(ByRef aMapWin As IMapWin, ByVal aX As Double, ByVal aY As Double) As atcCollection
        Dim lZ As Double
        Dim lR, lC As Integer
        Dim lOutCollection As New atcCollection

        For lLayerIndex As Integer = 0 To aMapWin.Layers.NumLayers - 1
            If aMapWin.Layers(lLayerIndex).LayerType = eLayerType.Grid Then
                Dim lGrid As MapWinGIS.Grid = aMapWin.Layers(lLayerIndex).GetGridObject
                lGrid.ProjToCell(aX, aY, lC, lR)
                lZ = lGrid.Value(lC, lR)
                If lZ > 0 Then
                    lOutCollection.Add(lLayerIndex, lZ)
                End If
            End If
        Next
        Return lOutCollection
    End Function
End Module
