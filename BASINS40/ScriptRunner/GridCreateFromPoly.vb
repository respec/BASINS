Imports atcUtility
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinGeoProc
Imports atcMwGisUtility

Module ScriptGridCreateFromPoly
    Private pTestPath As String = "D:\GisData\Illinois Snow\Excerpt"
    Private pPolyIdFieldName As String = "FIPS"
    Private pPolyName = "County"
  
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("Start")
        ChDriveDir(pTestPath)  'change to the directory of the current project
        Logger.Dbg(" CurDir:" & CurDir())
        GisUtil.MappingObject = aMapWin

        'Dim lBaseGridLayerIndex As Integer = GisUtil.LayerIndex("subset_us_ssmv11036tS__T0001TTNATS2004010205HP001.dat")
        Dim lBaseGridLayerIndex As Integer = GisUtil.LayerIndex("subset_us_ssmv11036tS__T0001TTNATS2004010605HP001.dat")
        Dim lBaseGrid As MapWinGIS.Grid = aMapWin.Layers(lBaseGridLayerIndex).GetGridObject

        Dim lPolyIdGridName As String = "PolyIdGrid.tif"
        If Not FileExists(lPolyIdGridName) Then 'create it
            Dim lPolyLayerIndex As Integer = GisUtil.LayerIndex(pPolyName)
            Dim lPolyLayer As MapWinGIS.Shapefile = aMapWin.Layers(lPolyLayerIndex).GetObject
            Dim lPolyFieldIndex As Integer = 0 'TODO:get index from name

            Dim lVals(lBaseGrid.Header.NumberCols) As Single
            For lRow As Integer = 1 To lBaseGrid.Header.NumberRows
                For lCol As Integer = 1 To lBaseGrid.Header.NumberCols
                    Dim lX, ly As Double
                    lBaseGrid.CellToProj(lCol, lRow, lX, ly)
                    Dim lPolyIndex As Integer = GisUtil.PointInPolygonXY(lX, ly, lPolyLayerIndex)
                    lVals(lCol - 1) = lPolyIndex 'lPolyLayer.CellValue(lPolyFieldIndex, lPolyIndex)
                Next
                lBaseGrid.PutRow(lRow, lVals(0))
            Next
            lBaseGrid.Save(lPolyIdGridName, MapWinGIS.GridFileType.GeoTiff)
            Logger.Dbg("DoneGridCreateFromPoly")
        End If

        Dim lPolyIdGrid As New MapWinGIS.Grid
        lPolyIdGrid.Open(lPolyIdGridName)
        Dim lPolyIdGridNoData As Integer = lPolyIdGrid.Header.NodataValue

        Dim lSum(lPolyIdGrid.Maximum) As Double
        Dim lCount(lPolyIdGrid.Maximum) As Integer
        Dim lCountBaseNoData As Integer = 0
        Dim lCountPolyNoData As Integer = 0

        'refresh the base grid
        lBaseGrid = aMapWin.Layers(lBaseGridLayerIndex).GetGridObject
        Dim lBaseGridNoData As Integer = lPolyIdGrid.Header.NodataValue
        Logger.Dbg("Base:Min:" & DoubleToString(lBaseGrid.Minimum) & " Max: " & DoubleToString(lBaseGrid.Maximum))

        For lRow As Integer = 1 To lPolyIdGrid.Header.NumberRows
            For lCol As Integer = 1 To lPolyIdGrid.Header.NumberCols
                Dim lPolyIndex As Integer = lPolyIdGrid.Value(lCol, lRow)
                If lPolyIndex <> lPolyIdGridNoData Then
                    Dim lValue As Double = lBaseGrid.Value(lCol, lRow)
                    If lValue <> lBaseGridNoData Then
                        lSum(lPolyIndex) += lValue
                        lCount(lPolyIndex) += 1
                    Else
                        lCountBaseNoData += 1
                    End If
                Else
                    lCountPolyNoData += 1
                End If
            Next
        Next

        Dim lAver As Double
        For lIndex As Integer = 0 To lPolyIdGrid.Maximum
            If lCount(lIndex) > 0 Then
                lAver = lSum(lIndex) / lCount(lIndex)
            Else
                lAver = 0
            End If
            Logger.Dbg("Id:" & lIndex & " Aver:" & DoubleToString(lAver) & " Sum:" & lSum(lIndex) & " Count:" & lCount(lIndex))
        Next
        Logger.Dbg("Skip:PolyNoData:" & lCountPolyNoData & " BaseNoData:" & lCountBaseNoData)
    End Sub
End Module
