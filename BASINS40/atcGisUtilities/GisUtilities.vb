Imports atcUtility
Imports MapWinUtility
Imports Microsoft.VisualBasic
Imports atcData
Imports atcMwGisUtility

''' <remarks>Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license</remarks>
''' <summary>GIS Utilities</summary>
Public Class GisUtilities
    
    Public Shared Sub GridSourceToShapefile(ByVal aMapWin As Object, ByVal aShapefileName As String, ByVal aSource As atcTimeseriesGridSource, ByVal aXfieldName As String, ByVal aYfieldName As String, ByVal aOutputProjection As String)
        'given an atcTimeseriesGridSource, build a shapefile 

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
            Else
                lAttributeNames.Add(aSource.CellValue(lRow, 0))
            End If
        Next

        'now populate collections of values
        For lCol As Integer = 1 To aSource.Columns - 1
            For lRow As Integer = 0 To aSource.Rows - 1
                If lRow = lXFieldPos Then
                    lXPositions.Add(aSource.CellValue(lRow, lCol))
                ElseIf lRow = lYFieldPos Then
                    lYPositions.Add(aSource.CellValue(lRow, lCol))
                Else
                    lAttributeValues.Add(aSource.CellValue(lRow, lCol))
                End If
            Next
        Next

        'delete old version of this shapefile if it exists
        If FileExists(aShapefileName) Then
            System.IO.File.Delete(aShapefileName)
        End If
        If FileExists(FilenameNoExt(aShapefileName) & ".shx") Then
            System.IO.File.Delete(FilenameNoExt(aShapefileName) & ".shx")
        End If
        If FileExists(FilenameNoExt(aShapefileName) & ".dbf") Then
            System.IO.File.Delete(FilenameNoExt(aShapefileName) & ".dbf")
        End If

        GisUtil.CreatePointShapefile(aShapefileName, lXPositions, lYPositions, lAttributeNames, lAttributeValues, aOutputProjection)

    End Sub

End Class


