Imports MapWinUtility
Imports MapWindow.Interfaces
Imports atcUtility
Imports atcClimateAssessmentTool

Module CatRunner
    Private pBaseFolder As String = "D:\Basins\data\Climate"
    Private pCatXMLFile As String = "VaryPrecTempHbnQuickPet.xml"
    Private WithEvents pCat As New atcClimateAssessmentTool.clsCat

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pBaseFolder)
        Logger.StartToFile("CatRunner.Log", , , True)
        With pCat
            .XML = IO.File.ReadAllText(pCatXMLFile)
            .StartRun("Modified")
            Logger.Dbg("RunsComplete")
            IO.File.WriteAllText("CatRunnerResults.txt", .ResultsGrid.ToString)
            .Inputs.Clear()
            .Endpoints.Clear()
            .PreparedInputs.Clear()
        End With
        Logger.Dbg("ResultsStored")
        Logger.Flush()
    End Sub

    Private Sub UpdateStatusLabel(ByVal aIteration As Integer) Handles pCat.StartIteration
        pCat.StartIterationMessage(aIteration)
    End Sub
End Module
