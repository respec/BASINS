Public Class RegionShape
    Inherits Region

    Private pShapeFilename As String
    Private pShapeKeyFieldName As String
    Private pShapeKeyField As Integer
    Private pShapeKeys As Generic.List(Of String)

    Sub New(ByVal aShapefilename As String, ByVal aShapeKeyFieldName As String, ByVal aShapeKeys As Generic.List(Of String))
        MyBase.New(GetBoundingXML(aShapefilename, aShapeKeyFieldName, aShapeKeys))
        pShapeFilename = aShapefilename
        pShapeKeyFieldName = aShapeKeyFieldName
        pShapeKeys = aShapeKeys
    End Sub

    Private Shared Function GetBoundingXML(ByVal aShapefilename As String, ByVal aShapeKeyField As Integer, ByVal aShapeKeys As Generic.List(Of String)) As Xml.XmlNode
        Dim lDocument As New Xml.XmlDocument
        'TODO: determine bounding box and load correct values
        lDocument.LoadXml("<region><northbc>0</northbc><southbc>0</southbc><eastbc>0</eastbc><westbc>0</westbc>")
        Return lDocument
    End Function

    Public Overrides Function GetKeysOfOverlappingShapes(ByVal aSelectFromShapeFilename As String, _
                                                         ByVal aKeyField As String, _
                                                         ByVal aSelectFromShapeProjection As String) As Generic.List(Of String)
        If aSelectFromShapeFilename.ToLower.Equals(pShapeFilename.ToLower) Then
            Return pShapeKeys
        Else
            Return MyBase.GetKeysOfOverlappingShapes(aSelectFromShapeFilename, aKeyField, aSelectFromShapeProjection)
        End If
    End Function

    Friend Overrides Function GetKeysOfOverlappingShapes(ByVal aSelectFromLayer As MapWinGIS.Shapefile, _
                                                           ByVal aKeyField As Integer, _
                                                           ByVal aSelectFromShapeProjection As String) As Generic.List(Of String)
        Dim lKeys As New Generic.List(Of String)
        Dim lLastShape As Integer = aSelectFromLayer.NumShapes - 1
        Dim lLastPoint As Integer
        Dim lShape As MapWinGIS.Shape
        Dim lPoint As MapWinGIS.Point

        'TODO: overlapping shapes, not just bounding box
        Dim lYMax As Double = pNorth
        Dim lYMin As Double = pSouth
        Dim lXMax As Double = pEast
        Dim lXMin As Double = pWest
        Dim lSwap As Double

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
        Return lKeys
    End Function

    <CLSCompliant(False)> _
    Public Overrides Function ToShape(ByVal aNewProjection As String) As MapWinGIS.Shape
        ToShape = Nothing
        Dim lShapeFile As New MapWinGIS.Shapefile
        If lShapeFile.Open(pShapeFilename, Nothing) Then
            For lShapeIndex As Integer = 1 To lShapeFile.NumShapes
                If pShapeKeys.Contains(lShapeFile.CellValue(pShapeKeyField, lShapeIndex)) Then
                    If ToShape Is Nothing Then
                        ToShape = lShapeFile.Shape(lShapeIndex)
                    Else
                        ToShape.InsertPart(ToShape.numPoints, ToShape.NumParts)
                        'TODO: finish inserting parts of matching shape
                    End If
                    ToShape = lShapeFile.Shape(lShapeIndex)
                End If
            Next
        End If
    End Function

    Public Overrides Function ToString() As String
        Return "Region " & pShapeKeys.Count & " Shapes from file '" & pShapeFilename & "'"
    End Function

End Class
