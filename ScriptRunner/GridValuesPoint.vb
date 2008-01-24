Imports atcData
Imports atcData.atcDataGroup
Imports atcUtility
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinGeoProc
Imports atcMwGisUtility

Imports Microsoft.VisualBasic
Imports System

Public Module ScriptGridValuesPoint
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

        Dim lXSectShapeName As String = "CrossSections"
        Dim lXSectLayerIndex As Integer = -1
        If GisUtil.IsLayer(CurDir() & "\" & lXSectShapeName & ".shp") Then
            lXSectLayerIndex = GisUtil.LayerIndex(CurDir() & "\" & lXSectShapeName & ".shp")
        ElseIf GisUtil.IsLayer(lXSectShapeName & ".shp") Then
            lXSectLayerIndex = GisUtil.LayerIndex(lXSectShapeName & ".shp")
        End If
        If lXSectLayerIndex > -1 Then
            GisUtil.RemoveLayer(lXSectLayerIndex)
        End If

        If FileExists(lXSectShapeName & ".shp") Then Kill(lXSectShapeName & ".shp")
        If FileExists(lXSectShapeName & ".shx") Then Kill(lXSectShapeName & ".shx")
        If FileExists(lXSectShapeName & ".dbf") Then Kill(lXSectShapeName & ".dbf")

        Dim lXSects As New MapWinGIS.Shapefile
        lXSects.CreateNew(lXSectShapeName & ".shp", MapWinGIS.ShpfileType.SHP_POLYLINEZ)
        Dim lFieldDefinitionX As New MapWinGIS.Field
        lFieldDefinitionX.Name = "X"
        lFieldDefinitionX.Type = MapWinGIS.FieldType.DOUBLE_FIELD
        lFieldDefinitionX.Width = 16
        lFieldDefinitionX.Precision = 0
        lXSects.EditInsertField(lFieldDefinitionX, 0)
        Dim lFieldDefinitionY As New MapWinGIS.Field
        lFieldDefinitionY.Name = "Y"
        lFieldDefinitionY.Type = MapWinGIS.FieldType.DOUBLE_FIELD
        lFieldDefinitionY.Width = 16
        lFieldDefinitionY.Precision = 0
        lXSects.EditInsertField(lFieldDefinitionY, 1)
        Dim lFieldDefinitionZ As New MapWinGIS.Field
        lFieldDefinitionZ.Name = "Z"
        lFieldDefinitionZ.Type = MapWinGIS.FieldType.DOUBLE_FIELD
        lFieldDefinitionZ.Width = 16
        lFieldDefinitionZ.Precision = 5
        lXSects.EditInsertField(lFieldDefinitionZ, 2)
        Dim lFieldDefinitionZOffset As New MapWinGIS.Field
        lFieldDefinitionZOffset.Name = "ZOffset"
        lFieldDefinitionZOffset.Type = MapWinGIS.FieldType.DOUBLE_FIELD
        lFieldDefinitionZOffset.Width = 16
        lFieldDefinitionZOffset.Precision = 5
        lXSects.EditInsertField(lFieldDefinitionZOffset, 4)
        Dim lFieldDefinitionZOrig As New MapWinGIS.Field
        lFieldDefinitionZOrig.Name = "ZOrig"
        lFieldDefinitionZOrig.Type = MapWinGIS.FieldType.DOUBLE_FIELD
        lFieldDefinitionZOrig.Width = 16
        lFieldDefinitionZOrig.Precision = 5
        lXSects.EditInsertField(lFieldDefinitionZOrig, 5)
        Dim lFieldDefinitionComId As New MapWinGIS.Field
        lFieldDefinitionComId.Name = "ComId"
        lFieldDefinitionComId.Type = MapWinGIS.FieldType.INTEGER_FIELD
        lFieldDefinitionComId.Width = 16
        lFieldDefinitionComId.Precision = 8
        lXSects.EditInsertField(lFieldDefinitionComId, 6)
        Dim lFieldDefinitionName As New MapWinGIS.Field
        lFieldDefinitionName.Name = "Name"
        lFieldDefinitionName.Type = MapWinGIS.FieldType.STRING_FIELD
        lFieldDefinitionName.Width = 16
        lXSects.EditInsertField(lFieldDefinitionName, 6)
        lXSects.StartEditingTable()
        lXSects.StartEditingShapes(True)

        Dim lXSectionString As New Text.StringBuilder
        lXSectionString.AppendLine("StepSize " & pStepSize & " StepCount " & pStepCount)
        lXSectionString.Append("Id" & vbTab & "Name" & vbTab & "ComId" & vbTab & "Offset")
        For lPointIndex As Integer = 0 To pStepCount
            lXSectionString.Append(vbTab & lPointIndex * pStepSize)
        Next
        lXSectionString.Append(vbCrLf)

        GisUtil.MappingObject = aMapWin

        Dim lLayerIndex As Integer = GisUtil.LayerIndex("NHDFlowline")
        Logger.Dbg("NhdFlowline:Index" & lLayerIndex & ":ShapeCount:" & GisUtil.NumFeatures(lLayerIndex))
        Dim lLayerIndexPoint As Integer = GisUtil.LayerIndex("ECMI Stage/Velocity")
        Logger.Dbg("Point:Index" & lLayerIndexPoint & ":ShapeCount:" & GisUtil.NumFeatures(lLayerIndexPoint))

        Dim lComIdFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, "COMID")
        'Dim lComId As Integer = 3434322 'downstream COMID for Upitoi Creek from visual inspection
        'WS22 Little Pine Knot	3432186
        'WS12 Sally Branch		3432066
        'WS14 Upatoi Creek		3430164
        'WS11 Bonham Creek		3432174
        'WS21 Randall Creek 	3431994
        'WS20 Oswichee Creek	3432838
        Dim lComIds() As Integer = {3432186, 3432066, 3430164, 3432174, 3431994, 3432838, 0}
        Dim lShapeIds() As Integer = {2, 3, 4, 5, 6, 7}
        Dim lComIdIndex As Integer = 0
        Dim lComId As Integer = lComIds(lComIdIndex)

        Do While lComId > 0
            Dim lShapeId As Integer = lShapeIds(lComIdIndex)
            Dim lFeatureIndexPoint As Integer = GisUtil.FindFeatureIndex(lLayerIndexPoint, 0, lShapeId.ToString)
            Dim lName As String = GisUtil.FieldValue(lLayerIndexPoint, lFeatureIndexPoint, 1)
            Dim lFeatureIndex As Integer = GisUtil.FindFeatureIndex(lLayerIndex, lComIdFieldIndex, lComId.ToString)
            Dim lXArray(1) As Double
            Dim lYArray(1) As Double
            Dim lX, lY, lZMin, lZMinOffset, lXCenter, lYCenter, lZCenter, dX, dY, dH, lRatio As Double
            GisUtil.PointXY(lLayerIndexPoint, lFeatureIndexPoint, lX, lY)
            lXArray(0) = lX
            lYArray(0) = lY
            GisUtil.FindNearestPointAndLoc(lX, lY, lLayerIndex, lFeatureIndex)
            lXArray(1) = lX
            lYArray(1) = lY
            dX = lXArray(1) - lXArray(0)
            dY = lYArray(1) - lYArray(0)
            dH = Math.Sqrt((dX ^ 2) + (dY ^ 2)) 'meters
            'build a crosssection for this segment by extending the line
            lRatio = pStepSize / dH
            lXCenter = lXArray(1)
            lX = lXCenter + (dX * lRatio * (pStepCount / 2)) 'x offset to start of Xsection
            lYCenter = lYArray(1)
            lY = lYCenter + (dY * lRatio * (pStepCount / 2)) 'y offset to start of Xsection

            Dim lXSect As New MapWinGIS.Shape
            Dim lPointCount As Integer = 0
            lXSect.Create(MapWinGIS.ShpfileType.SHP_POLYLINEZ)
            lZMin = 1.0E+30
            lXSectionString.Append(lComIdIndex & vbTab & lName & vbTab & lComId & vbTab)
            For lPointIndex As Integer = 0 To pStepCount
                Dim lPointCollection As atcCollection = XY2Z(aMapWin, lX, lY)
                If lPointCollection.Count > 0 Then
                    Dim lPoint As New MapWinGIS.Point
                    lPoint.x = lX
                    lPoint.y = lY
                    lPoint.Z = lPointCollection.ItemByIndex(lPointCollection.Count - 1)
                    lXSectionString.Append(vbTab & DoubleToString(lPoint.Z, , pFormat, , , 8))
                    If lPoint.Z < lZMin Then
                        lZMin = lPoint.Z
                        lZMinOffset = Math.Abs(lPointIndex - (pStepCount / 2)) * pStepSize
                    End If
                    If lPointIndex = pStepCount / 2 Then 'elevation from orig nhd flowline
                        lZCenter = lPoint.Z
                    End If
                    lXSect.InsertPoint(lPoint, lPointIndex)
                End If
                lX += -(dX * lRatio)
                lY += -(dY * lRatio)
            Next
            lXSectionString.AppendLine()
            lXSects.EditInsertShape(lXSect, lComIdIndex)
            lXSects.EditCellValue(0, lComIdIndex, lXCenter) ' x point on stream line
            lXSects.EditCellValue(1, lComIdIndex, lYCenter) ' y point of stream line
            lXSects.EditCellValue(2, lComIdIndex, lZMin)
            lXSects.EditCellValue(3, lComIdIndex, lZMinOffset)
            lXSects.EditCellValue(4, lComIdIndex, lZCenter)
            lXSects.EditCellValue(5, lComIdIndex, lComId)
            lXSects.EditCellValue(6, lComIdIndex, lName)
            lComIdIndex += 1
            If lComIdIndex <= lComIds.GetUpperBound(0) Then
                lComId = lComIds(lComIdIndex)
            Else
                lComId = 0
            End If
        Loop
        lXSects.StopEditingShapes(True)
        lXSects.StopEditingTable(True)
        lXSects.SaveAs(lXSectShapeName & ".shp")
        GisUtil.AddLayer(lXSectShapeName & ".shp", "Cross Sections")
        GisUtil.LayerVisible(lXSectShapeName & ".shp") = True

        SaveFileString("XSection.txt", lXSectionString.ToString)
    End Sub

    Function XY2Z(ByRef aMapWin As IMapWin, ByVal aX As Double, ByVal aY As Double) As atcCollection
        Dim lZ As Double
        Dim lR, lC As Integer
        Dim lOutCollection As New atcCollection

        For lLayerIndex As Integer = 0 To aMapWin.Layers.NumLayers - 1
            If GisUtil.LayerType(lLayerIndex) = eLayerType.Grid Then
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
