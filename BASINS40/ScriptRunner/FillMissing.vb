Imports atcUtility
Imports atcData
Imports atcWDM
Imports MapWindow.Interfaces

Module FillMissing
    Private Const pTestPath As String = "D:\Basins\Data\"
    Private Const pBaseName As String = "Upatoi"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pTestPath)
        'open WDM file
        Dim lWdmFileName As String = IO.Path.Combine(pTestPath, pBaseName & ".wdm")
        Dim lWdmDataSource As New atcDataSourceWDM
        lWdmDataSource.Open(lWdmFileName)
        Dim lFlowTser As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(4)
        Dim lFilledTser As atcTimeseries = FillMissingByInterpolation(lFlowTser, 1)
        lWdmDataSource.AddDataset(lFilledTser)
    End Sub
End Module
