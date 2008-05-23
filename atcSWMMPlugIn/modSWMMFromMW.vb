Imports atcMwGisUtility
Imports atcSWMM

Friend Module modSWMMFromMW
    Public Function CreateCatchmentsFromShapefile(ByVal aShapefileName As String, ByVal aSubbasinFieldName As String, ByVal aSlopeFieldName As String, _
                                                  ByVal aSWMMProject As SWMMProject, ByRef aCatchments As Catchments) As Boolean
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
            'lCatchment.RainGage()

            'find associated conduit
            For Each lConduit As Conduit In aSWMMProject.Conduits
                If lConduit.Name.Substring(1) = lSubbasinID Then
                    lCatchment.Conduit = lConduit
                    Exit For
                End If
            Next

            lCatchment.FeatureIndex = lFeatureIndex
            lCatchment.Area = GisUtil.FeatureArea(lLayerIndex, lFeatureIndex) / 4047.0  'convert m2 to acres
            'lCatchment.PercentImpervious()
            'lCatchment.Width()
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

        'create all conduits
        For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lLayerIndex) - 1
            Dim lConduit As New Conduit
            lConduit.Name = "C" & GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lSubbasinFieldIndex)
            lConduit.FeatureIndex = lFeatureIndex

            lConduit.Length = GisUtil.FeatureLength(lLayerIndex, lFeatureIndex)
            'lConduit.ManningsN()
            'lConduit.InletOffset()
            'lConduit.OutletOffset()
            'lConduit.InitialFlow()
            'lConduit.MaxFlow()

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
End Module
