Imports MapWinUtility
Imports MapWindow.Interfaces
Imports atcUtility
Imports atcClimateAssessmentTool

Module CatRunner
    Private pBaseFolder As String = "D:\Basins\data\Climate"
    Private pCatXMLFile As String = "VaryPrecTempHbnQuickJLK.xml"
    Private WithEvents pCat As New atcClimateAssessmentTool.clsCat

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pBaseFolder)
        Logger.StartToFile("CatRunner.Log", , , True)
        pCat.XML = IO.File.ReadAllText(pCatXMLFile)
        pCat.StartRun("Base.uci", "Modified")
        Logger.Dbg("RunsComplete")
        IO.File.WriteAllText("CatRunnerResults.txt", pCat.ResultsGrid.ToString)
        Logger.Dbg("ResultsStored")
        Logger.Flush()
    End Sub

    Private Sub UpdateStatusLabel(ByVal aIteration As Integer) Handles pCat.StartIteration
        Logger.Dbg("StartIteration " & aIteration + 1 & " of " & pCat.TotalIterations)
    End Sub
End Module
