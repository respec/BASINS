Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcUtility
Imports atcData
Imports atcManDelin
Imports System.IO
Imports System.Text
Public Module PopulateStreamAttributes

    Private Const pInputPath As String = "C:\Dev\CherryCreek\Modeling\BASINS_CherryCreek\Watershed"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())
        Dim lStreamsLayerName As String = "stream11"
        Dim lSubbasinLayerName As String = "subbasin8"
        Dim lElevationLayerName As String = "10190003demg_2"
        Dim lElevUnits As String = "meters"
        Dim lTmp As Integer = 0
        ManDelinPlugIn.CalculateReachParameters(lStreamsLayerName, lSubbasinLayerName, lElevationLayerName, lElevUnits, lTmp)

    End Sub
End Module
