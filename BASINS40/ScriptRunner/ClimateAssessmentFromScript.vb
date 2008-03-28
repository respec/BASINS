Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData
Imports atcClimateAssessmentTool
Imports atcWDM

Module ClimateAssessmentFromScript
    Private Const pTestPath As String = "D:\Basins\data\Climate"
    Private Const pBaseName As String = "base"
    Private Const pScenName As String = "scriptDemo"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lResults As New atcCollection

        ChDriveDir(pTestPath)

        Dim lOriginalData As New atcDataSourceWDM
        lOriginalData.Open(pBaseName & ".wdm")
        Dim lOriginalPrecip As atcTimeseries = lOriginalData.DataSets.FindData("ID", "105").Item(0)

        Dim lVariation As New atcVariation
        With lVariation
            .DataSets.Add(lOriginalPrecip)
            .Name = "Adjust Precip"
            .ComputationSource = New atcTimeseriesMath.atcTimeseriesMath
            .Operation = "Multiply"
            .Min = 1.1
            .Max = 1.1
            .Increment = 0.1
            Dim lModifiedData As atcDataGroup = .StartIteration()
            lResults = modCAT.ScenarioRun(pBaseName & ".uci", pScenName, lModifiedData, "", True, True, True)
            Logger.Dbg("AllDone:" & lResults.Count)
        End With
    End Sub
End Module
