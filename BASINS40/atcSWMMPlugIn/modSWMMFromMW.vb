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
End Module
