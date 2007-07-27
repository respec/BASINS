Imports atcUtility
Imports MapWinUtility
Imports Microsoft.VisualBasic
Imports atcData
Imports atcMwGisUtility

''' <remarks>Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license</remarks>
''' <summary>GIS Utilities</summary>
Public Class GisUtilities

    ''' <summary>
    ''' given an atcTimeseriesGridSource, build a shapefile 
    ''' </summary>
    ''' <param name="aMapWin"></param>
    ''' <param name="aShapefileName"></param>
    ''' <param name="aSource"></param>
    ''' <param name="aXfieldName"></param>
    ''' <param name="aYfieldName"></param>
    ''' <param name="aOutputProjection"></param>
    ''' <remarks></remarks>
    Public Shared Sub GridSourceToShapefile(ByVal aMapWin As Object, _
                                            ByVal aShapefileName As String, _
                                            ByVal aSource As atcTimeseriesGridSource, _
                                            ByVal aXfieldName As String, ByVal aYfieldName As String, _
                                            ByVal aOutputProjection As String)
        GisUtil.MappingObject = aMapWin

        Dim lXPositions As New Collection
        Dim lYPositions As New Collection
        Dim lAttributeNames As New Collection
        Dim lAttributeValues As New Collection

        'find the x and y fields
        Dim lXFieldPos As Integer = 0
        Dim lYFieldPos As Integer = 0
        For lRow As Integer = 0 To aSource.Rows - 1
            If UCase(aSource.CellValue(lRow, 0)) = aXfieldName.ToUpper Then
                lXFieldPos = lRow
            ElseIf UCase(aSource.CellValue(lRow, 0)) = aYfieldName.ToUpper Then
                lYFieldPos = lRow
            End If
            'save attribute names
            lAttributeNames.Add(aSource.CellValue(lRow, 0))
        Next

        'now populate collections of values
        For lCol As Integer = 1 To aSource.Columns - 1
            For lRow As Integer = 0 To aSource.Rows - 1
                If lRow = lXFieldPos Then
                    lXPositions.Add(aSource.CellValue(lRow, lCol))
                ElseIf lRow = lYFieldPos Then
                    lYPositions.Add(aSource.CellValue(lRow, lCol))
                End If
                'save attribute value
                lAttributeValues.Add(aSource.CellValue(lRow, lCol))
            Next
        Next

        'delete old version of this shapefile if it exists
        TryDeleteShapefile(aShapefileName)

        GisUtil.CreatePointShapefile(aShapefileName, lXPositions, lYPositions, lAttributeNames, lAttributeValues, aOutputProjection)

    End Sub

End Class


