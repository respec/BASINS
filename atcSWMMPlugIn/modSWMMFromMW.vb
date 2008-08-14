Imports atcMwGisUtility
Imports atcSWMM
Imports atcData
Imports MapWinUtility
Imports atcUtility

Friend Module modSWMMFromMW

    Public Function CompleteCatchmentsFromShapefile(ByVal aCatchmentShapefileName As String, _
                                                    ByVal aSWMMProject As SWMMProject, _
                                                    ByRef aCatchments As Catchments) As Boolean

        If Not GisUtil.IsLayerByFileName(aCatchmentShapefileName) Then
            GisUtil.AddLayer(aCatchmentShapefileName, "Catchments")
        End If
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(aCatchmentShapefileName)
        
        For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lLayerIndex) - 1
            Dim lCatchment As Catchment = aCatchments(lFeatureIndex)

            If lCatchment.Name.Length = 0 Then
                lCatchment.Name = CStr(lFeatureIndex + 1)
            Else
                If IsNumeric(CDbl(lCatchment.Name)) Then
                    lCatchment.Name = CStr(CInt(lCatchment.Name))
                End If
            End If

            If aSWMMProject.RainGages.Count > 0 Then
                'assign each catchment to the first raingage for now
                lCatchment.RainGage = aSWMMProject.RainGages(0)
            End If

            'find associated outlet node
            If Not lCatchment.OutletNodeID Is Nothing Then
                If aSWMMProject.Nodes.Contains(lCatchment.OutletNodeID) Then
                    lCatchment.OutletNode = aSWMMProject.Nodes(lCatchment.OutletNodeID)
                End If
            End If

            If lCatchment.OutletNode Is Nothing Then
                'find node through associated conduit
                For Each lConduit As Conduit In aSWMMProject.Conduits
                    If lConduit.Name.Substring(1) = lCatchment.Name Then
                        lCatchment.OutletNode = lConduit.OutletNode
                        Exit For
                    End If
                Next
            End If

            lCatchment.Area = GisUtil.FeatureArea(lLayerIndex, lFeatureIndex) / 4047.0  'convert m2 to acres
            'lCatchment.PercentImpervious()  'this is computed later
            lCatchment.Width = Math.Sqrt(lCatchment.Area * 43560)

            lCatchment.Name = "S" & lCatchment.Name

            GisUtil.PointsOfLine(lLayerIndex, lFeatureIndex, lCatchment.X, lCatchment.Y)
        Next
    End Function

    Public Function CompleteConduitsFromShapefile(ByVal aConduitShapefilename As String, _
                                                  ByVal aSWMMProject As SWMMProject, _
                                                  ByRef aConduits As Conduits) As Boolean

        Dim lNeedNodes As Boolean = False
        If aSWMMProject.Nodes.Count = 0 Then
            lNeedNodes = True
        End If

        If Not GisUtil.IsLayerByFileName(aConduitShapefilename) Then
            GisUtil.AddLayer(aConduitShapefilename, "Conduits")
        End If
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(aConduitShapefilename)

        For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lLayerIndex) - 1
            Dim lConduit As Conduit = aConduits(lFeatureIndex)

            'calculate the actual feature length
            lConduit.Length.Value = GisUtil.FeatureLength(lLayerIndex, lFeatureIndex) * 3.281 'need to convert meters to feet

            If lConduit.Geometry1 < 0 Then
                lConduit.Geometry1 = lConduit.MeanDepth * 1.25 'full height = mean depth * 1.25
            End If
            If lConduit.Geometry2 < 0 Then
                lConduit.Geometry2 = lConduit.MeanWidth - (lConduit.MeanDepth / lConduit.Geometry3) - (lConduit.MeanDepth / lConduit.Geometry4) 'base width
            End If

            If lNeedNodes Then
                'nodes not specified, create them at end points of conduits

                If lConduit.ElevationHigh < lConduit.ElevationLow Then
                    'something is wrong, switch places
                    Dim lTemp As Double = lConduit.ElevationHigh
                    lConduit.ElevationHigh = lConduit.ElevationLow
                    lConduit.ElevationLow = lTemp
                End If

                Dim lXup As Double
                Dim lYup As Double
                Dim lXdown As Double
                Dim lYdown As Double
                GisUtil.EndPointsOfLine(lLayerIndex, lFeatureIndex, lXup, lYup, lXdown, lYdown)
                'todo: may need to verify which way the line is digitized

                'create node at upstream end
                Dim lUpNode As New Node
                lUpNode.Name = "J" & lConduit.Name
                lUpNode.Type = "JUNCTION"
                lUpNode.InvertElevation = lConduit.ElevationHigh
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
                If lConduit.DownConduitID.Length > 0 And CInt(lConduit.DownConduitID) > 0 Then
                    lNode.Name = "J" & lConduit.DownConduitID
                    lNode.Type = "JUNCTION"
                Else
                    lNode.Name = "O1"
                    lNode.Type = "OUTFALL"
                End If
                lNode.InvertElevation = lConduit.ElevationLow
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
                If aSWMMProject.Nodes.Contains(lConduit.InletNodeName) Then
                    lConduit.InletNode = aSWMMProject.Nodes(lConduit.InletNodeName)
                End If

                If aSWMMProject.Nodes.Contains(lConduit.OutletNodeName) Then
                    lConduit.OutletNode = aSWMMProject.Nodes(lConduit.OutletNodeName)
                End If
            End If

            'if it doesnt have a name yet, assign it one
            If lConduit.Name.Length = 0 Then
                lConduit.Name = "C" & CStr(lFeatureIndex + 1)
            ElseIf IsNumeric(lConduit.Name) Then
                lConduit.Name = "C" & lConduit.Name
            End If

            'find the coordinates
            GisUtil.PointsOfLine(lLayerIndex, lFeatureIndex, lConduit.X, lConduit.Y)
        Next

    End Function

    Public Function CompleteNodesFromShapefile(ByVal aNodeShapefileName As String, _
                                               ByRef aNodes As Nodes) As Boolean

        If aNodes.Count > 0 Then
            If Not GisUtil.IsLayerByFileName(aNodeShapefileName) Then
                GisUtil.AddLayer(aNodeShapefileName, "Nodes")
            End If
            Dim lLayerIndex As Integer = GisUtil.LayerIndex(aNodeShapefileName)

            For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lLayerIndex) - 1
                Dim lNode As Node = aNodes(lFeatureIndex)
                'if it doesnt have a name yet, assign it one
                If lNode.Name.Length = 0 Then
                    lNode.Name = "N" & CInt(lFeatureIndex)
                End If
                'find the coordinates
                GisUtil.PointXY(lLayerIndex, lFeatureIndex, lNode.XPos, lNode.YPos)
            Next
        End If

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
