Imports atcMwGisUtility
Imports atcSWMM
Imports atcData
Imports MapWinUtility

Friend Module modSWMMFromMW
    Public Function CreateCatchmentsFromShapefile(ByVal aShapefileName As String, ByVal aSubbasinFieldName As String, ByVal aSlopeFieldName As String, _
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
            'lCatchment.PercentImpervious()
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
                                                ByVal aSWMMProject As SWMMProject, _
                                                ByRef aConduits As Conduits) As Boolean
        aConduits.Clear()

        If Not GisUtil.IsLayerByFileName(aShapefileName) Then
            GisUtil.AddLayer(aShapefileName, "Conduits")
        End If
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(aShapefileName)
        Dim lSubbasinFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, aSubbasinFieldName)
        Dim lDownSubbasinFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, aDownSubbasinFieldName)
        Dim lElevHighFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, aElevHighFieldName)
        Dim lElevLowFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, aElevLowFieldName)
        Dim lMeanWidthFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, aMeanWidthFieldName)
        Dim lMeanDepthFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, aMeanDepthFieldName)

        'create all conduits
        For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lLayerIndex) - 1
            Dim lConduit As New Conduit
            lConduit.Name = "C" & GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lSubbasinFieldIndex)

            lConduit.Length = GisUtil.FeatureLength(lLayerIndex, lFeatureIndex) * 3.281 'need to convert meters to feet
            'lConduit.ManningsN()
            'lConduit.InletOffset()
            'lConduit.OutletOffset()
            'lConduit.InitialFlow()
            'lConduit.MaxFlow()
            'lconduit.Shape
            lConduit.Geometry3 = 1.0 'left slope
            lConduit.Geometry4 = 1.0 'right slope
            Dim lMeanDepth As Double = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lMeanDepthFieldIndex)
            lConduit.Geometry1 = lMeanDepth * 1.25 'full height = mean depth * 1.25
            lConduit.Geometry2 = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lMeanWidthFieldIndex) - (lMeanDepth / lConduit.Geometry3) - (lMeanDepth / lConduit.Geometry4) 'base width
            'lConduit.NumBarrels()

            Dim lElevHigh As Double = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lElevHighFieldIndex)
            Dim lElevLow As Double = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lElevLowFieldIndex)
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
            Dim lSubID As String = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lSubbasinFieldIndex)
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
            Dim lDownID As String = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lDownSubbasinFieldIndex)
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

            GisUtil.PointsOfLine(lLayerIndex, lFeatureIndex, lConduit.X, lConduit.Y)

            aConduits.Add(lConduit)
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

End Module
