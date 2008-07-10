Imports atcMwGisUtility
Imports atcSWMM
Imports atcData
Imports MapWinUtility

Friend Module modSWMMFromMW
    Public Function CreateCatchmentsFromShapefile(ByVal aCatchmentSpecs As CatchmentShapefileSpecs, _
                                                  ByVal aSWMMProject As SWMMProject, _
                                                  ByRef aCatchments As Catchments) As Boolean

        aCatchments.Clear()
        If Not GisUtil.IsLayerByFileName(aCatchmentSpecs.ShapefileName) Then
            GisUtil.AddLayer(aCatchmentSpecs.ShapefileName, "Catchments")
        End If
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(aCatchmentSpecs.ShapefileName)
        Dim lSubbasinFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.SubbasinFieldName)
        Dim lSlopeFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.SlopeFieldName)

        'this field normally exists when using a nodes shapefile
        Dim lOutletNodeFieldIndex As Integer = -1
        If aCatchmentSpecs.OutletNodeFieldName.Length > 0 Then lOutletNodeFieldIndex = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.OutletNodeFieldName)

        'these fields may exist for some users
        Dim lWidthFieldIndex As Integer = -1
        If aCatchmentSpecs.WidthFieldName.Length > 0 Then lWidthFieldIndex = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.WidthFieldName)
        Dim lCurbLengthFieldIndex As Integer = -1
        If aCatchmentSpecs.CurbLengthFieldName.Length > 0 Then lCurbLengthFieldIndex = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.CurbLengthFieldName)
        Dim lSnowPackNameFieldIndex As Integer = -1
        If aCatchmentSpecs.SnowPackNameFieldName.Length > 0 Then lSnowPackNameFieldIndex = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.SnowPackNameFieldName)
        Dim lManningsNImpervFieldIndex As Integer = -1
        If aCatchmentSpecs.ManningsNImpervFieldName.Length > 0 Then lManningsNImpervFieldIndex = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.ManningsNImpervFieldName)
        Dim lManningsNPervFieldIndex As Integer = -1
        If aCatchmentSpecs.ManningsNPervFieldName.Length > 0 Then lManningsNPervFieldIndex = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.ManningsNPervFieldName)
        Dim lDepressionStorageImpervFieldIndex As Integer = -1
        If aCatchmentSpecs.DepressionStorageImpervFieldName.Length > 0 Then lDepressionStorageImpervFieldIndex = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.DepressionStorageImpervFieldName)
        Dim lDepressionStoragePervFieldIndex As Integer = -1
        If aCatchmentSpecs.DepressionStoragePervFieldName.Length > 0 Then lDepressionStoragePervFieldIndex = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.DepressionStoragePervFieldName)
        Dim lPercentZeroStorageFieldIndex As Integer = -1
        If aCatchmentSpecs.PercentZeroStorageFieldName.Length > 0 Then lPercentZeroStorageFieldIndex = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.PercentZeroStorageFieldName)
        Dim lRouteToFieldIndex As Integer = -1
        If aCatchmentSpecs.RouteToFieldName.Length > 0 Then lRouteToFieldIndex = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.RouteToFieldName)
        Dim lPercentRoutedFieldIndex As Integer = -1
        If aCatchmentSpecs.PercentRoutedFieldName.Length > 0 Then lPercentRoutedFieldIndex = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.PercentRoutedFieldName)
        Dim lMaxInfiltRateFieldIndex As Integer = -1
        If aCatchmentSpecs.MaxInfiltRateFieldName.Length > 0 Then lMaxInfiltRateFieldIndex = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.MaxInfiltRateFieldName)
        Dim lMinInfiltRateFieldIndex As Integer = -1
        If aCatchmentSpecs.MinInfiltRateFieldName.Length > 0 Then lMinInfiltRateFieldIndex = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.MinInfiltRateFieldName)
        Dim lDecayRateConstantFieldIndex As Integer = -1
        If aCatchmentSpecs.DecayRateConstantFieldName.Length > 0 Then lDecayRateConstantFieldIndex = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.DecayRateConstantFieldName)
        Dim lDryTimeFieldIndex As Integer = -1
        If aCatchmentSpecs.DryTimeFieldName.Length > 0 Then lDryTimeFieldIndex = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.DryTimeFieldName)
        Dim lMaxInfiltVolumeFieldIndex As Integer = -1
        If aCatchmentSpecs.MaxInfiltVolumeFieldName.Length > 0 Then lMaxInfiltVolumeFieldIndex = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.MaxInfiltVolumeFieldName)
        Dim lSuctionFieldIndex As Integer = -1
        If aCatchmentSpecs.SuctionFieldName.Length > 0 Then lSuctionFieldIndex = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.SuctionFieldName)
        Dim lConductivityFieldIndex As Integer = -1
        If aCatchmentSpecs.ConductivityFieldName.Length > 0 Then lConductivityFieldIndex = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.ConductivityFieldName)
        Dim lInitialDeficitFieldIndex As Integer = -1
        If aCatchmentSpecs.InitialDeficitFieldName.Length > 0 Then lInitialDeficitFieldIndex = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.InitialDeficitFieldName)
        Dim lCurveNumberFieldIndex As Integer = -1
        If aCatchmentSpecs.CurveNumberFieldName.Length > 0 Then lCurveNumberFieldIndex = GisUtil.FieldIndex(lLayerIndex, aCatchmentSpecs.CurveNumberFieldName)

        For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lLayerIndex) - 1
            Dim lCatchment As New Catchment
            Dim lSubbasinID As String = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lSubbasinFieldIndex)
            lCatchment.Name = "S" & lSubbasinID
            If aSWMMProject.RainGages.Count > 0 Then
                'assign each catchment to the first raingage for now
                lCatchment.RainGage = aSWMMProject.RainGages(0)
            End If

            'find associated outlet node
            Dim lOutletNode As String = ""
            If lOutletNodeFieldIndex > -1 Then
                lOutletNode = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lOutletNodeFieldIndex)
            End If
            If aSWMMProject.Nodes.Contains(lOutletNode) Then
                lCatchment.OutletNode = aSWMMProject.Nodes(lOutletNode)
            End If

            If lCatchment.OutletNode Is Nothing Then
                'find node through associated conduit
                For Each lConduit As Conduit In aSWMMProject.Conduits
                    If lConduit.Name.Substring(1) = lSubbasinID Then
                        lCatchment.OutletNode = lConduit.OutletNode
                        Exit For
                    End If
                Next
            End If

            lCatchment.Area = GisUtil.FeatureArea(lLayerIndex, lFeatureIndex) / 4047.0  'convert m2 to acres
            'lCatchment.PercentImpervious()  'this is computed later
            lCatchment.Width = Math.Sqrt(lCatchment.Area * 43560)
            If lSlopeFieldIndex > 0 Then
                lCatchment.Slope = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lSlopeFieldIndex)
            End If
            If lWidthFieldIndex > -1 Then
                lCatchment.Width = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lWidthFieldIndex)
            End If
            If lCurbLengthFieldIndex > -1 Then
                lCatchment.CurbLength = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lCurbLengthFieldIndex)
            End If
            If lSnowPackNameFieldIndex > -1 Then
                lCatchment.SnowPackName = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lSnowPackNameFieldIndex)
            End If
            If lManningsNImpervFieldIndex > -1 Then
                lCatchment.ManningsNImperv = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lManningsNImpervFieldIndex)
            End If
            If lManningsNPervFieldIndex > -1 Then
                lCatchment.ManningsNPerv = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lManningsNPervFieldIndex)
            End If
            If lDepressionStorageImpervFieldIndex > -1 Then
                lCatchment.DepressionStorageImperv = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lDepressionStorageImpervFieldIndex)
            End If
            If lDepressionStoragePervFieldIndex > -1 Then
                lCatchment.DepressionStoragePerv = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lDepressionStoragePervFieldIndex)
            End If
            If lPercentZeroStorageFieldIndex > -1 Then
                lCatchment.PercentZeroStorage = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lPercentZeroStorageFieldIndex)
            End If
            If lRouteToFieldIndex > -1 Then
                lCatchment.RouteTo = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lRouteToFieldIndex)
            End If
            If lPercentRoutedFieldIndex > -1 Then
                lCatchment.PercentRouted = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lPercentRoutedFieldIndex)
            End If
            If lMaxInfiltRateFieldIndex > -1 Then
                lCatchment.MaxInfiltRate = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lMaxInfiltRateFieldIndex)
            End If
            If lMinInfiltRateFieldIndex > -1 Then
                lCatchment.MinInfiltRate = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lMinInfiltRateFieldIndex)
            End If
            If lDecayRateConstantFieldIndex > -1 Then
                lCatchment.DecayRateConstant = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lDecayRateConstantFieldIndex)
            End If
            If lDryTimeFieldIndex > -1 Then
                lCatchment.DryTime = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lDryTimeFieldIndex)
            End If
            If lMaxInfiltVolumeFieldIndex > -1 Then
                lCatchment.MaxInfiltVolume = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lMaxInfiltVolumeFieldIndex)
            End If
            If lSuctionFieldIndex > -1 Then
                lCatchment.Suction = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lSuctionFieldIndex)
            End If
            If lConductivityFieldIndex > -1 Then
                lCatchment.Conductivity = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lConductivityFieldIndex)
            End If
            If lInitialDeficitFieldIndex > -1 Then
                lCatchment.InitialDeficit = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lInitialDeficitFieldIndex)
            End If
            If lCurveNumberFieldIndex > -1 Then
                lCatchment.CurveNumber = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lCurveNumberFieldIndex)
            End If

            GisUtil.PointsOfLine(lLayerIndex, lFeatureIndex, lCatchment.X, lCatchment.Y)
            aCatchments.Add(lCatchment)
        Next
    End Function

    Public Function CreateConduitsFromShapefile(ByVal aConduitSpecs As ConduitShapefileSpecs, _
                                                ByVal aSWMMProject As SWMMProject, _
                                                ByRef aConduits As Conduits) As Boolean
        aConduits.Clear()

        If Not GisUtil.IsLayerByFileName(aConduitSpecs.ShapefileName) Then
            GisUtil.AddLayer(aConduitSpecs.ShapefileName, "Conduits")
        End If
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(aConduitSpecs.ShapefileName)

        'these fields normally exist when using a basins delineation
        Dim lSubbasinFieldIndex As Integer = -1
        If aConduitSpecs.SubbasinFieldName.Length > 0 Then lSubbasinFieldIndex = GisUtil.FieldIndex(lLayerIndex, aConduitSpecs.SubbasinFieldName)
        Dim lDownSubbasinFieldIndex As Integer = -1
        If aConduitSpecs.DownSubbasinFieldName.Length > 0 Then lDownSubbasinFieldIndex = GisUtil.FieldIndex(lLayerIndex, aConduitSpecs.DownSubbasinFieldName)
        Dim lElevHighFieldIndex As Integer = -1
        If aConduitSpecs.ElevHighFieldName.Length > 0 Then lElevHighFieldIndex = GisUtil.FieldIndex(lLayerIndex, aConduitSpecs.ElevHighFieldName)
        Dim lElevLowFieldIndex As Integer = -1
        If aConduitSpecs.ElevLowFieldName.Length > 0 Then lElevLowFieldIndex = GisUtil.FieldIndex(lLayerIndex, aConduitSpecs.ElevLowFieldName)
        Dim lMeanWidthFieldIndex As Integer = -1
        If aConduitSpecs.MeanWidthFieldName.Length > 0 Then lMeanWidthFieldIndex = GisUtil.FieldIndex(lLayerIndex, aConduitSpecs.MeanWidthFieldName)
        Dim lMeanDepthFieldIndex As Integer = -1
        If aConduitSpecs.MeanDepthFieldName.Length > 0 Then lMeanDepthFieldIndex = GisUtil.FieldIndex(lLayerIndex, aConduitSpecs.MeanDepthFieldName)

        'these fields normally exist when using a nodes shapefile
        Dim lInletNodeFieldIndex As Integer = -1
        If aConduitSpecs.InletNodeFieldName.Length > 0 Then lInletNodeFieldIndex = GisUtil.FieldIndex(lLayerIndex, aConduitSpecs.InletNodeFieldName)
        Dim lOutletNodeFieldIndex As Integer = -1
        If aConduitSpecs.OutletNodeFieldName.Length > 0 Then lOutletNodeFieldIndex = GisUtil.FieldIndex(lLayerIndex, aConduitSpecs.OutletNodeFieldName)

        'these fields may exist for some users
        Dim lManningsNFieldIndex As Integer = -1
        If aConduitSpecs.ManningsNFieldName.Length > 0 Then lManningsNFieldIndex = GisUtil.FieldIndex(lLayerIndex, aConduitSpecs.ManningsNFieldName)
        Dim lInletOffsetFieldIndex As Integer = -1
        If aConduitSpecs.InletOffsetFieldName.Length > 0 Then lInletOffsetFieldIndex = GisUtil.FieldIndex(lLayerIndex, aConduitSpecs.InletOffsetFieldName)
        Dim lOutletOffsetFieldIndex As Integer = -1
        If aConduitSpecs.OutletOffsetFieldName.Length > 0 Then lOutletOffsetFieldIndex = GisUtil.FieldIndex(lLayerIndex, aConduitSpecs.OutletOffsetFieldName)
        Dim lInitialFlowFieldIndex As Integer = -1
        If aConduitSpecs.InitialFlowFieldName.Length > 0 Then lInitialFlowFieldIndex = GisUtil.FieldIndex(lLayerIndex, aConduitSpecs.InitialFlowFieldName)
        Dim lMaxFlowFieldIndex As Integer = -1
        If aConduitSpecs.MaxFlowFieldName.Length > 0 Then lMaxFlowFieldIndex = GisUtil.FieldIndex(lLayerIndex, aConduitSpecs.MaxFlowFieldName)
        Dim lShapeFieldIndex As Integer = -1
        If aConduitSpecs.ShapeFieldName.Length > 0 Then lShapeFieldIndex = GisUtil.FieldIndex(lLayerIndex, aConduitSpecs.ShapeFieldName)
        Dim lGeometry1FieldIndex As Integer = -1
        If aConduitSpecs.Geometry1FieldName.Length > 0 Then lGeometry1FieldIndex = GisUtil.FieldIndex(lLayerIndex, aConduitSpecs.Geometry1FieldName)
        Dim lGeometry2FieldIndex As Integer = -1
        If aConduitSpecs.Geometry2FieldName.Length > 0 Then lGeometry2FieldIndex = GisUtil.FieldIndex(lLayerIndex, aConduitSpecs.Geometry2FieldName)
        Dim lGeometry3FieldIndex As Integer = -1
        If aConduitSpecs.Geometry3FieldName.Length > 0 Then lGeometry3FieldIndex = GisUtil.FieldIndex(lLayerIndex, aConduitSpecs.Geometry3FieldName)
        Dim lGeometry4FieldIndex As Integer = -1
        If aConduitSpecs.Geometry4FieldName.Length > 0 Then lGeometry4FieldIndex = GisUtil.FieldIndex(lLayerIndex, aConduitSpecs.Geometry4FieldName)
        Dim lNumBarrelsFieldIndex As Integer = -1
        If aConduitSpecs.NumBarrelsFieldName.Length > 0 Then lNumBarrelsFieldIndex = GisUtil.FieldIndex(lLayerIndex, aConduitSpecs.NumBarrelsFieldName)

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

            If aConduitSpecs.CreateNodes Then
                'nodes not specified, create them at end points of conduits
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
            Else
                'use nodes from shapefile 
                Dim lInletNode As String = ""
                If lInletNodeFieldIndex > -1 Then
                    lInletNode = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lInletNodeFieldIndex)
                End If
                If aSWMMProject.Nodes.Contains(lInletNode) Then
                    lConduit.InletNode = aSWMMProject.Nodes(lInletNode)
                End If

                Dim lOutletNode As String = ""
                If lOutletNodeFieldIndex > -1 Then
                    lOutletNode = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lOutletNodeFieldIndex)
                End If
                If aSWMMProject.Nodes.Contains(lOutletNode) Then
                    lConduit.OutletNode = aSWMMProject.Nodes(lOutletNode)
                End If
            End If

            GisUtil.PointsOfLine(lLayerIndex, lFeatureIndex, lConduit.X, lConduit.Y)

            aConduits.Add(lConduit)
        Next

    End Function

    Public Function CreateNodesFromShapefile(ByVal aNodeSpecs As NodeShapefileSpecs, _
                                             ByRef aNodes As Nodes) As Boolean

        aNodes.Clear()

        If Not GisUtil.IsLayerByFileName(aNodeSpecs.ShapefileName) Then
            GisUtil.AddLayer(aNodeSpecs.ShapefileName, "Nodes")
        End If
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(aNodeSpecs.ShapefileName)

        Dim lNameFieldIndex As Integer = -1
        If aNodeSpecs.NameFieldName.Length > 0 Then lNameFieldIndex = GisUtil.FieldIndex(lLayerIndex, aNodeSpecs.NameFieldName)
        Dim lTypeFieldIndex As Integer = -1
        If aNodeSpecs.TypeFieldName.Length > 0 Then lTypeFieldIndex = GisUtil.FieldIndex(lLayerIndex, aNodeSpecs.TypeFieldName)
        Dim lInvertElevationFieldIndex As Integer = -1
        If aNodeSpecs.InvertElevationFieldName.Length > 0 Then lInvertElevationFieldIndex = GisUtil.FieldIndex(lLayerIndex, aNodeSpecs.InvertElevationFieldName)
        Dim lMaxDepthFieldIndex As Integer = -1
        If aNodeSpecs.MaxDepthFieldName.Length > 0 Then lMaxDepthFieldIndex = GisUtil.FieldIndex(lLayerIndex, aNodeSpecs.MaxDepthFieldName)
        Dim lInitDepthFieldIndex As Integer = -1
        If aNodeSpecs.InitDepthFieldName.Length > 0 Then lInitDepthFieldIndex = GisUtil.FieldIndex(lLayerIndex, aNodeSpecs.InitDepthFieldName)
        Dim lSurchargeDepthFieldIndex As Integer = -1
        If aNodeSpecs.SurchargeDepthFieldName.Length > 0 Then lSurchargeDepthFieldIndex = GisUtil.FieldIndex(lLayerIndex, aNodeSpecs.SurchargeDepthFieldName)
        Dim lPondedAreaFieldIndex As Integer = -1
        If aNodeSpecs.PondedAreaFieldName.Length > 0 Then lPondedAreaFieldIndex = GisUtil.FieldIndex(lLayerIndex, aNodeSpecs.PondedAreaFieldName)
        Dim lOutfallTypeFieldIndex As Integer = -1
        If aNodeSpecs.OutfallTypeFieldName.Length > 0 Then lOutfallTypeFieldIndex = GisUtil.FieldIndex(lLayerIndex, aNodeSpecs.OutfallTypeFieldName)
        Dim lStageTableFieldIndex As Integer = -1
        If aNodeSpecs.StageTableFieldName.Length > 0 Then lStageTableFieldIndex = GisUtil.FieldIndex(lLayerIndex, aNodeSpecs.StageTableFieldName)
        Dim lTideGateFieldIndex As Integer = -1
        If aNodeSpecs.TideGateFieldName.Length > 0 Then lTideGateFieldIndex = GisUtil.FieldIndex(lLayerIndex, aNodeSpecs.TideGateFieldName)

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
            If lStageTableFieldIndex > -1 Then
                lNode.StageTable = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lStageTableFieldIndex)
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
