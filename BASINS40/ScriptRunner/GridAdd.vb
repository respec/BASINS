Imports atcUtility
Imports atcMwGisUtility
Imports MapWindow.Interfaces
Imports MapWinUtility

Public Module ScriptGridAdd
    Private pTestPath As String = "D:\GisData\SERDP"
    Private pStepSize As Integer = 2 'meters
    Private pStepCount As Integer = 20 'leads to 40m wide cross section
    Private Const pFormat As String = "#,##0.00"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("Start")
        'change to the directory of the current project
        ChDriveDir(pTestPath)
        Logger.Dbg(" CurDir:" & CurDir())

        GisUtil.MappingObject = aMapWin
        Dim lBaseGridLayerIndex As Integer = GisUtil.LayerIndex("1000")
        Dim lColoringScheme As MapWinGIS.GridColorScheme = aMapWin.Layers(lBaseGridLayerIndex).GetGridObject.RasterColorTableColoringScheme

        Dim lLayerIndex As Integer = GisUtil.LayerIndex("UpatoiOverlay")
        Dim lTileCount = GisUtil.NumFeatures(lLayerIndex)
        Logger.Dbg("Found UpatoiOverlay:ShapeCount:" & lTileCount)

        Dim lTileIdFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, "tile_num")
        For lShapeIndex As Integer = 0 To lTileCount - 1
            Dim lTileId As String = GisUtil.FieldValue(lLayerIndex, lShapeIndex, lTileIdFieldIndex)
            Logger.Dbg("Process " & lTileId)
            Try
                Dim lTileLayerId As Integer = GisUtil.LayerIndex(lTileId)
                Logger.Dbg("AlreadyInProjectAsLayer " & lTileLayerId)
                'would be nice to reset the renderer here 
                'aMapWin.Layers(lTileLayerId).GetGridObject.RasterColorTableColoringScheme = lColoringScheme
            Catch 'add layer
                Dim lTileName As String = pTestPath & "\Lidar\Float_Grid_Extract\" & lTileId & ".flt"
                If Not FileExists(lTileName) Then
                    FileCopy(lTileName.Replace("D:\GisData", "Z:"), lTileName)
                    FileCopy(lTileName.Replace("D:\GisData", "Z:").Replace(".flt", ".hdr"), lTileName.Replace(".flt", ".hdr"))
                End If
                'Logger.Dbg("Open " & lTileName)
                'Dim lTile As New MapWinGIS.Grid
                'Dim lStatus As Boolean = lTile.Open(lTileName)
                'Logger.Dbg("  Open " & lStatus)
                'aMapWin.Layers.Add(lTile, lColoringScheme, lTileID)
                Logger.Dbg("Add " & lTileName)
                aMapWin.Layers.Add(lTileName, lTileId)
                aMapWin.Project.Save(aMapWin.Project.FileName)
            End Try
            Logger.Dbg("Done," & MemUsage())
        Next
    End Sub
    Private Function MemUsage() As String
        System.GC.WaitForPendingFinalizers()
        Return "MemoryUsage(MB):" & DoubleToString(Process.GetCurrentProcess.PrivateMemorySize64 / (2 ^ 20), , pFormat) & _
                    " Local(MB):" & DoubleToString(System.GC.GetTotalMemory(True) / (2 ^ 20), , pFormat)
    End Function
End Module
