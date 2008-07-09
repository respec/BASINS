Imports atcMwGisUtility
Imports atcSWMM
Imports atcData
Imports MapWinUtility

Friend Module modSWMMFromMW
    Public Function CreateCatchmentsFromShapefile(ByVal aShapefileName As String, _
                                                  ByVal aSubbasinFieldName As String, _
                                                  ByVal aSlopeFieldName As String, _
                                                  ByVal aSWMMProject As SWMMProject, _
                                                  ByRef aCatchments As Catchments) As Boolean

        aCatchments.Clear()
        If Not GisUtil.IsLayerByFileName(aShapefileName) Then
            GisUtil.AddLayer(aShapefileName, "Catchments")
        End If
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(aShapefileName)
        Dim lSubbasinFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, aSubbasinFieldName)
        Dim lSlopeFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, aSlopeFieldName)

        For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lLayerIndex) - 1
            Dim lCatchment As New Catchment
            Dim lSubbasinID As String = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lSubbasinFieldIndex)
            lCatchment.Name = "S" & lSubbasinID
            If aSWMMProject.RainGages.Count > 0 Then
                'assign each catchment to the first raingage for now
                lCatchment.RainGage = aSWMMProject.RainGages(0)
            End If

            'find associated conduit
            For Each lConduit As Conduit In aSWMMProject.Conduits
                If lConduit.Name.Substring(1) = lSubbasinID Then
                    lCatchment.Conduit = lConduit
                    Exit For
                End If
            Next

            lCatchment.Area = GisUtil.FeatureArea(lLayerIndex, lFeatureIndex) / 4047.0  'convert m2 to acres
            'lCatchment.PercentImpervious()  'this is computed later
            lCatchment.Width = lCatchment.Area * 43560 / lCatchment.Conduit.Length
            lCatchment.Slope = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lSlopeFieldIndex)
            GisUtil.PointsOfLine(lLayerIndex, lFeatureIndex, lCatchment.X, lCatchment.Y)
            aCatchments.Add(lCatchment)
        Next
    End Function

    Public Function CreateConduitsFromShapefile(ByVal aShapefileName As String, _
                                                ByVal aSubbasinFieldName As String, _
                                                ByVal aDownSubbasinFieldName As String, _
                                                ByVal aElevHighFieldName As String, _
                                                ByVal aElevLowFieldName As String, _
                                                ByVal aMeanWidthFieldName As String, _
                                                ByVal aMeanDepthFieldName As String, _
                                                ByVal aManningsNFieldName As String, _
                                                ByVal aInletOffsetFieldName As String, _
                                                ByVal aOutletOffsetFieldName As String, _
                                                ByVal aInitialFlowFieldName As String, _
                                                ByVal aMaxFlowFieldName As String, _
                                                ByVal aShapeFieldName As String, _
                                                ByVal aGeometry1FieldName As String, _
                                                ByVal aGeometry2FieldName As String, _
                                                ByVal aGeometry3FieldName As String, _
                                                ByVal aGeometry4FieldName As String, _
                                                ByVal aNumBarrelsFieldName As String, _
                                                ByVal aCreateNodes As Boolean, _
                                                ByVal aSWMMProject As SWMMProject, _
                                                ByRef aConduits As Conduits) As Boolean
        aConduits.Clear()

        If Not GisUtil.IsLayerByFileName(aShapefileName) Then
            GisUtil.AddLayer(aShapefileName, "Conduits")
        End If
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(aShapefileName)

        'these fields normally exist when using a basins delineation
        Dim lSubbasinFieldIndex As Integer = -1
        If aSubbasinFieldName.Length > 0 Then lSubbasinFieldIndex = GisUtil.FieldIndex(lLayerIndex, aSubbasinFieldName)
        Dim lDownSubbasinFieldIndex As Integer = -1
        If aDownSubbasinFieldName.Length > 0 Then lDownSubbasinFieldIndex = GisUtil.FieldIndex(lLayerIndex, aDownSubbasinFieldName)
        Dim lElevHighFieldIndex As Integer = -1
        If aElevHighFieldName.Length > 0 Then lElevHighFieldIndex = GisUtil.FieldIndex(lLayerIndex, aElevHighFieldName)
        Dim lElevLowFieldIndex As Integer = -1
        If aElevLowFieldName.Length > 0 Then lElevLowFieldIndex = GisUtil.FieldIndex(lLayerIndex, aElevLowFieldName)
        Dim lMeanWidthFieldIndex As Integer = -1
        If aMeanWidthFieldName.Length > 0 Then lMeanWidthFieldIndex = GisUtil.FieldIndex(lLayerIndex, aMeanWidthFieldName)
        Dim lMeanDepthFieldIndex As Integer = -1
        If aMeanDepthFieldName.Length > 0 Then lMeanDepthFieldIndex = GisUtil.FieldIndex(lLayerIndex, aMeanDepthFieldName)

        'these fields may exist for some users
        Dim lManningsNFieldIndex As Integer = -1
        If aManningsNFieldName.Length > 0 Then lManningsNFieldIndex = GisUtil.FieldIndex(lLayerIndex, aManningsNFieldName)
        Dim lInletOffsetFieldIndex As Integer = -1
        If aInletOffsetFieldName.Length > 0 Then lInletOffsetFieldIndex = GisUtil.FieldIndex(lLayerIndex, aInletOffsetFieldName)
        Dim lOutletOffsetFieldIndex As Integer = -1
        If aOutletOffsetFieldName.Length > 0 Then lOutletOffsetFieldIndex = GisUtil.FieldIndex(lLayerIndex, aOutletOffsetFieldName)
        Dim lInitialFlowFieldIndex As Integer = -1
        If aInitialFlowFieldName.Length > 0 Then lInitialFlowFieldIndex = GisUtil.FieldIndex(lLayerIndex, aInitialFlowFieldName)
        Dim lMaxFlowFieldIndex As Integer = -1
        If aMaxFlowFieldName.Length > 0 Then lMaxFlowFieldIndex = GisUtil.FieldIndex(lLayerIndex, aMaxFlowFieldName)
        Dim lShapeFieldIndex As Integer = -1
        If aShapeFieldName.Length > 0 Then lShapeFieldIndex = GisUtil.FieldIndex(lLayerIndex, aShapeFieldName)
        Dim lGeometry1FieldIndex As Integer = -1
        If aGeometry1FieldName.Length > 0 Then lGeometry1FieldIndex = GisUtil.FieldIndex(lLayerIndex, aGeometry1FieldName)
        Dim lGeometry2FieldIndex As Integer = -1
        If aGeometry2FieldName.Length > 0 Then lGeometry2FieldIndex = GisUtil.FieldIndex(lLayerIndex, aGeometry2FieldName)
        Dim lGeometry3FieldIndex As Integer = -1
        If aGeometry3FieldName.Length > 0 Then lGeometry3FieldIndex = GisUtil.FieldIndex(lLayerIndex, aGeometry3FieldName)
        Dim lGeometry4FieldIndex As Integer = -1
        If aGeometry4FieldName.Length > 0 Then lGeometry4FieldIndex = GisUtil.FieldIndex(lLayerIndex, aGeometry4FieldName)
        Dim lNumBarrelsFieldIndex As Integer = -1
        If aNumBarrelsFieldName.Length > 0 Then lNumBarrelsFieldIndex = GisUtil.FieldIndex(lLayerIndex, aNumBarrelsFieldName)

        'create all conduits
        For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lLayerIndex) - 1
            Dim lConduit As New Conduit
            If lSubbasinFieldIndex > -1 Then
                lConduit.Name = "C" & GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lSubbasinFieldIndex)
            Else
                lConduit.Name = "C" & CStr(lFeatureIndex + 1)
            End If

            lConduit.Length = GisUtil.FeatureLength(lLayerIndex, lFeatureIndex) * 3.281 'need to convert meters to feet

            If lManningsNFieldIndex > -1 Then
                lConduit.ManningsN = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lManningsNFieldIndex)
            End If
            If lInletOffsetFieldIndex > -1 Then
                lConduit.InletOffset = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lInletOffsetFieldIndex)
            End If
            If lOutletOffsetFieldIndex > -1 Then
                lConduit.OutletOffset = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lOutletOffsetFieldIndex)
            End If
            If lInitialFlowFieldIndex > -1 Then
                lConduit.InitialFlow = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lInitialFlowFieldIndex)
            End If
            If lMaxFlowFieldIndex > -1 Then
                lConduit.MaxFlow = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lMaxFlowFieldIndex)
            End If
            If lShapeFieldIndex > -1 Then
                lConduit.Shape = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lShapeFieldIndex)
            End If
            If lNumBarrelsFieldIndex > -1 Then
                lConduit.NumBarrels = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lNumBarrelsFieldIndex)
            End If

            Dim lMeanDepth As Double = 0.0
            If lMeanDepthFieldIndex > -1 Then
                lMeanDepth = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lMeanDepthFieldIndex)
            End If
            Dim lMeanWidth As Double = 0.0
            If lMeanWidthFieldIndex > -1 Then
                lMeanWidth = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lMeanWidthFieldIndex)
            End If

            lConduit.Geometry3 = 1.0 'left slope
            lConduit.Geometry4 = 1.0 'right slope
            lConduit.Geometry1 = lMeanDepth * 1.25 'full height = mean depth * 1.25
            lConduit.Geometry2 = lMeanWidth - (lMeanDepth / lConduit.Geometry3) - (lMeanDepth / lConduit.Geometry4) 'base width

            If lGeometry1FieldIndex > -1 Then
                lConduit.Geometry1 = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lGeometry1FieldIndex)
            End If
            If lGeometry2FieldIndex > -1 Then
                lConduit.Geometry2 = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lGeometry2FieldIndex)
            End If
            If lGeometry3FieldIndex > -1 Then
                lConduit.Geometry3 = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lGeometry3FieldIndex)
            End If
            If lGeometry4FieldIndex > -1 Then
                lConduit.Geometry4 = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lGeometry4FieldIndex)
            End If

            If aCreateNodes Then
                Dim lElevHigh As Double = 0.0
                If lElevHighFieldIndex > -1 Then
                    lElevHigh = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lElevHighFieldIndex)
                End If

                Dim lElevLow As Double = 0.0
                If lElevLowFieldIndex > -1 Then
                    lElevLow = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lElevLowFieldIndex)
                End If

                If lElevHigh < lElevLow Then
                    'something is wrong, switch places
                    Dim lTemp As Double = lElevHigh
                    lElevHigh = lElevLow
                    lElevLow = lTemp
                End If

                Dim lXup As Double
                Dim lYup As Double
                Dim lXdown As Double
                Dim lYdown As Double
                GisUtil.EndPointsOfLine(lLayerIndex, lFeatureIndex, lXup, lYup, lXdown, lYdown)
                'todo: may need to verify which way the line is digitized

                'create node at upstream end
                Dim lUpNode As New Node
                Dim lSubID As String
                If lSubbasinFieldIndex > -1 Then
                    lSubID = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lSubbasinFieldIndex)
                Else
                    lSubID = CStr(lFeatureIndex + 1)
                End If
                lUpNode.Name = "J" & lSubID
                lUpNode.Type = "JUNCTION"
                lUpNode.InvertElevation = lElevHigh
                lUpNode.XPos = lXup
                lUpNode.YPos = lYup
                If Not aSWMMProject.Nodes.Contains(lUpNode.Name) Then
                    aSWMMProject.Nodes.Add(lUpNode)
                    lConduit.InletNode = lUpNode
                Else
                    lConduit.InletNode = aSWMMProject.Nodes(lUpNode.Name)
                End If

                'create node at downstream end
                Dim lNode As New Node
                Dim lDownID As String
                If lDownSubbasinFieldIndex > -1 Then
                    lDownID = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lDownSubbasinFieldIndex)
                Else
                    lDownID = CStr(lFeatureIndex + 1)
                End If
                If CInt(lDownID) > 0 Then
                    lNode.Name = "J" & lDownID
                    lNode.Type = "JUNCTION"
                Else
                    lNode.Name = "O1"
                    lNode.Type = "OUTFALL"
                End If
                lNode.InvertElevation = lElevLow
                lNode.XPos = lXdown
                lNode.YPos = lYdown
                If Not aSWMMProject.Nodes.Contains(lNode.Name) Then
                    aSWMMProject.Nodes.Add(lNode)
                    lConduit.OutletNode = lNode
                Else
                    lConduit.OutletNode = aSWMMProject.Nodes(lNode.Name)
                    'make sure coordinates correspond with downstream end
                    lConduit.OutletNode.XPos = lNode.XPos
                    lConduit.OutletNode.YPos = lNode.YPos
                End If
            End If

            GisUtil.PointsOfLine(lLayerIndex, lFeatureIndex, lConduit.X, lConduit.Y)

            aConduits.Add(lConduit)
        Next

    End Function

    Public Function CreateNodesFromShapefile(ByVal aShapefileName As String, _
                                             ByVal aNameFieldName As String, _
                                             ByVal aTypeFieldName As String, _
                                             ByVal aInvertElevationFieldName As String, _
                                             ByVal aMaxDepthFieldName As String, _
                                             ByVal aInitDepthFieldName As String, _
                                             ByVal aSurchargeDepthFieldName As String, _
                                             ByVal aPondedAreaFieldName As String, _
                                             ByVal aOutfallTypeFieldName As String, _
                                             ByVal aStageTableFieldName As String, _
                                             ByVal aTideGateFieldName As String, _
                                             ByRef aNodes As Nodes) As Boolean

        aNodes.Clear()

        If Not GisUtil.IsLayerByFileName(aShapefileName) Then
            GisUtil.AddLayer(aShapefileName, "Nodes")
        End If
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(aShapefileName)

        Dim lNameFieldIndex As Integer = -1
        If aNameFieldName.Length > 0 Then lNameFieldIndex = GisUtil.FieldIndex(lLayerIndex, aNameFieldName)
        Dim lTypeFieldIndex As Integer = -1
        If aTypeFieldName.Length > 0 Then lTypeFieldIndex = GisUtil.FieldIndex(lLayerIndex, aTypeFieldName)
        Dim lInvertElevationFieldIndex As Integer = -1
        If aInvertElevationFieldName.Length > 0 Then lInvertElevationFieldIndex = GisUtil.FieldIndex(lLayerIndex, aInvertElevationFieldName)
        Dim lMaxDepthFieldIndex As Integer = -1
        If aMaxDepthFieldName.Length > 0 Then lMaxDepthFieldIndex = GisUtil.FieldIndex(lLayerIndex, aMaxDepthFieldName)
        Dim lInitDepthFieldIndex As Integer = -1
        If aInitDepthFieldName.Length > 0 Then lInitDepthFieldIndex = GisUtil.FieldIndex(lLayerIndex, aInitDepthFieldName)
        Dim lSurchargeDepthFieldIndex As Integer = -1
        If aSurchargeDepthFieldName.Length > 0 Then lSurchargeDepthFieldIndex = GisUtil.FieldIndex(lLayerIndex, aSurchargeDepthFieldName)
        Dim lPondedAreaFieldIndex As Integer = -1
        If aPondedAreaFieldName.Length > 0 Then lPondedAreaFieldIndex = GisUtil.FieldIndex(lLayerIndex, aPondedAreaFieldName)
        Dim lOutfallTypeFieldIndex As Integer = -1
        If aOutfallTypeFieldName.Length > 0 Then lOutfallTypeFieldIndex = GisUtil.FieldIndex(lLayerIndex, aOutfallTypeFieldName)
        Dim lStageTableFieldIndex As Integer = -1
        If aStageTableFieldName.Length > 0 Then lStageTableFieldIndex = GisUtil.FieldIndex(lLayerIndex, aStageTableFieldName)
        Dim lTideGateFieldIndex As Integer = -1
        If aTideGateFieldName.Length > 0 Then lTideGateFieldIndex = GisUtil.FieldIndex(lLayerIndex, aTideGateFieldName)

        'create all nodes
        For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lLayerIndex) - 1
            Dim lNode As New Node

            If lNameFieldIndex > -1 Then
                lNode.Name = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lNameFieldIndex)
            End If
            If lNode.Name.Length = 0 Then
                lNode.Name = "N" & CInt(lFeatureIndex)
            End If

            If lTypeFieldIndex > -1 Then
                lNode.Type = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lTypeFieldIndex)
            End If
            If lInvertElevationFieldIndex > 0 Then
                lNode.InvertElevation = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lInvertElevationFieldIndex)
            End If
            If lMaxDepthFieldIndex > -1 Then
                lNode.MaxDepth = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lMaxDepthFieldIndex)
            End If
            If lInitDepthFieldIndex > -1 Then
                lNode.InitDepth = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lInitDepthFieldIndex)
            End If
            If lSurchargeDepthFieldIndex > -1 Then
                lNode.SurchargeDepth = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lSurchargeDepthFieldIndex)
            End If
            If lPondedAreaFieldIndex > -1 Then
                lNode.PondedArea = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lPondedAreaFieldIndex)
            End If
            If lOutfallTypeFieldIndex > -1 Then
                lNode.OutfallType = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lOutfallTypeFieldIndex)
            End If
            If lNameFieldIndex > -1 Then
                lNode.StageTable = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lNameFieldIndex)
            End If
            If lTideGateFieldIndex > -1 Then
                lNode.TideGate = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lTideGateFieldIndex)
            End If

            GisUtil.PointXY(lLayerIndex, lFeatureIndex, lNode.XPos, lNode.YPos)

            aNodes.Add(lNode)
        Next

    End Function

    Public Function CreateRaingageFromShapefile(ByVal aShapefileName As String, _
                                                ByVal aGageId As String, _
                                                ByRef aRainGages As RainGages) As Boolean

        If Not GisUtil.IsLayerByFileName(aShapefileName) Then
            GisUtil.AddLayer(aShapefileName, "Raingages")
        End If
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(aShapefileName)
        Dim lGageIDFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, "LOCATION")
        Dim lWDMFileName As String = FilenameNoExt(aShapefileName) & ".wdm"
        For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lLayerIndex) - 1
            If GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lGageIDFieldIndex) = aGageId Then
                Dim lRaingage As New RainGage
                lRaingage.Name = aGageId
                'lRaingage.Form
                'lRaingage.Interval()
                'lRaingage.SnowCatchFactor()
                'lRaingage.Type
                'lRaingage.Units
                lRaingage.TimeSeries = GetTimeseries(lWDMFileName, "OBSERVED", aGageId, "PREC")
                If lRaingage.TimeSeries = Nothing Then
                    lRaingage.TimeSeries = GetTimeseries(lWDMFileName, "COMPUTED", aGageId, "PREC")
                End If
                GisUtil.PointXY(lLayerIndex, lFeatureIndex, lRaingage.XPos, lRaingage.YPos)
                If Not aRainGages.Contains(lRaingage.Name) Then
                    aRainGages.Add(lRaingage)
                End If
            End If
        Next

    End Function

    Public Function CreateLandusesFromGrid(ByVal aLanduseGridFileName As String, _
                                           ByVal aSubbasinShapefileName As String, _
                                           ByVal aCatchments As Catchments, _
                                           ByRef aLanduses As Landuses) As Boolean

        aLanduses.Clear()

        If Not GisUtil.IsLayerByFileName(aSubbasinShapefileName) Then
            GisUtil.AddLayer(aSubbasinShapefileName, "Catchments")
        End If
        Dim lSubbasinLayerIndex As Integer = GisUtil.LayerIndex(aSubbasinShapefileName)

        If Not GisUtil.IsLayerByFileName(aLanduseGridFileName) Then
            GisUtil.AddLayer(aLanduseGridFileName, "Landuse Grid")
        End If
        Dim lLanduseLayerIndex As Integer = GisUtil.LayerIndex(aLanduseGridFileName)

        Dim lMaxLanduseCategory As Integer = Convert.ToInt32(GisUtil.GridLayerMaximum(lLanduseLayerIndex))
        Dim lAreaLS(lMaxLanduseCategory, GisUtil.NumFeatures(lSubbasinLayerIndex)) As Double
        GisUtil.TabulateAreas(lLanduseLayerIndex, lSubbasinLayerIndex, lAreaLS)

        For lCatchmentIndex As Integer = 0 To aCatchments.Count - 1
            For lLanduseCategory As Integer = 1 To lMaxLanduseCategory
                If lAreaLS(lLanduseCategory, lCatchmentIndex) > 0 Then
                    Dim lLanduse As New Landuse
                    lLanduse.Area = lAreaLS(lLanduseCategory, lCatchmentIndex)
                    lLanduse.Name = lLanduseCategory
                    lLanduse.Catchment = aCatchments(lCatchmentIndex)
                    aLanduses.Add(lLanduse)
                End If
            Next
        Next

        ComputeImperviousPercentage(aCatchments, aLanduses)

    End Function

    Public Function CreateMetConstituent(ByVal aWDMFileName As String, _
                                         ByVal aGageId As String, _
                                         ByVal aConstituentID As String, _
                                         ByRef aMetConstituents As MetConstituents) As Boolean

        Dim lMetConstituent As New MetConstituent
        lMetConstituent.TimeSeries = GetTimeseries(aWDMFileName, "OBSERVED", aGageId, aConstituentID)
        If lMetConstituent.TimeSeries = Nothing Then
            lMetConstituent.TimeSeries = GetTimeseries(aWDMFileName, "COMPUTED", aGageId, aConstituentID)
        End If
        lMetConstituent.Type = aConstituentID
        If Not aMetConstituents.Contains(aConstituentID) Then
            aMetConstituents.Add(lMetConstituent)
        End If

    End Function

    Private Function GetTimeseries(ByRef aMetWDMName As String, _
                                   ByVal aScenario As String, _
                                   ByVal aLocation As String, _
                                   ByVal aConstituent As String) As atcData.atcTimeseries
        Dim lGetTimeseries As atcData.atcTimeseries = Nothing

        Dim lDataSource As New atcWDM.atcDataSourceWDM
        If FileExists(aMetWDMName) Then
            Dim lFound As Boolean = False
            For Each lBASINSDataSource As atcDataSource In atcDataManager.DataSources
                If lBASINSDataSource.Specification.ToUpper = aMetWDMName.ToUpper Then
                    'found it in the BASINS data sources
                    lDataSource = lBASINSDataSource
                    lFound = True
                    Exit For
                End If
            Next

            If Not lFound Then 'need to open it here
                If lDataSource.Open(aMetWDMName) Then
                    lFound = True
                End If
            End If

            If lFound Then
                For Each lDataSet As atcData.atcTimeseries In lDataSource.DataSets
                    If (lDataSet.Attributes.GetValue("Scenario") = aScenario And _
                        lDataSet.Attributes.GetValue("Constituent") = aConstituent And _
                        lDataSet.Attributes.GetValue("Location") = aLocation) Then
                        lGetTimeseries = lDataSet
                        Exit For
                    End If
                Next
            End If
        End If
        Return lGetTimeseries
    End Function

    Private Function ComputeImperviousPercentage(ByVal aCatchments As Catchments, _
                                                 ByVal aLanduses As Landuses) As Boolean

        For Each lCatchment As Catchment In aCatchments
            Dim lImperviousArea As Double = 0.0
            Dim lPerviousArea As Double = 0.0
            For Each lLanduse As Landuse In aLanduses
                If lLanduse.Catchment.Name = lCatchment.Name Then
                    'a match, store areas of pervious and impervious

                    'TODO: use table to determine % impervious for each
                    Dim lAddedArea As Boolean = False
                    If IsNumeric(lLanduse.Name) Then
                        If Int(lLanduse.Name) > 20 And Int(lLanduse.Name) < 25 Then
                            lImperviousArea += lLanduse.Area * 0.5
                            lPerviousArea += lLanduse.Area * 0.5
                            lAddedArea = True
                        End If
                    End If
                    If Not lAddedArea Then
                        lPerviousArea += lLanduse.Area
                    End If

                End If
            Next

            'compute the impervious percentage for this catchment
            lCatchment.PercentImpervious = 100.0 * lImperviousArea / (lImperviousArea + lPerviousArea)

        Next

    End Function

End Module
