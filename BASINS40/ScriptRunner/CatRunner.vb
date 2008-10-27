Imports MapWinUtility
Imports MapWindow.Interfaces
Imports atcUtility
Imports atcClimateAssessmentTool

Module CatRunner
    'Private pBaseFolder As String = "C:\mono_luChange\output\lu2030a2"
    Private pBaseFolder As String
    Private pBaseFolders As New ArrayList
    Private pCatXMLFile As String = "VaryPrecTempHbnPrepare.xml"
    Private WithEvents pCat As New atcClimateAssessmentTool.clsCat

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        'Add scenario directories
        pBaseFolders.Add("C:\mono_luChange\output\lu2030a2")
        pBaseFolders.Add("C:\mono_luChange\output\lu2030b2")
        pBaseFolders.Add("C:\mono_luChange\output\lu2090a2")
        pBaseFolders.Add("C:\mono_luChange\output\lu2090b2")
        pBaseFolders.Add("C:\mono_luChange\output\Mono10")
        pBaseFolders.Add("C:\mono_luChange\output\Mono70")

        For Each pBaseFolder In pBaseFolders
            ChDriveDir(pBaseFolder)
            Logger.StartToFile("CatRunner.Log", , , True)
            Logger.DisplayMessageBoxes = False
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
        Next
    End Sub

    Private Sub UpdateStatusLabel(ByVal aIteration As Integer) Handles pCat.StartIteration
        pCat.StartIterationMessage(aIteration)
    End Sub

    Private Sub UpdateResults(ByVal aResultsFilename As String) Handles pCat.UpdateResults
        Try
            Windows.Forms.Application.DoEvents()
        Catch
            'stop
        End Try
        SaveFileString(aResultsFilename, pCat.ResultsGrid.ToString)
    End Sub
End Module
