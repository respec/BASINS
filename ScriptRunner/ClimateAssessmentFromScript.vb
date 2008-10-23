Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData
Imports atcClimateAssessmentTool
Imports atcWDM

Module ClimateAssessmentFromScript
    Private Const pBaseFolder As String = "D:\Basins\data\Climate\"
    Private Const pBaseName As String = "base"
    Private WithEvents pCat As New atcClimateAssessmentTool.clsCat

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pBaseFolder)

        Dim lOriginalData As New atcDataSourceWDM
        lOriginalData.Open(pBaseName & ".wdm")

        With pCat
            .BaseScenario = pBaseFolder & pBaseName & ".uci"
            Dim lVariation As New atcVariation
            With lVariation
                .Name = "Adjust Precip"
                .Operation = "Multiply"
                .DataSets.Add(lOriginalData.DataSets.FindData("ID", "105").Item(0))
                .ComputationSource = New atcTimeseriesMath.atcTimeseriesMath
                .Min = 0.9
                .Max = 1.1
                .Increment = 0.1
                .Selected = True
            End With
            .Inputs.Add(lVariation)

            Dim lEndPoint As New atcVariation
            With lEndPoint
                .Name = "Prec"
                .Operation = "SumAnnual"
                .DataSets.Add(lOriginalData.DataSets.FindData("ID", "105").Item(0))
                .Selected = True
            End With
            .Endpoints.Add(lEndPoint)
            lEndPoint = New atcVariation
            With lEndPoint
                .Name = "Flow"
                .Operation = "Mean"
                .DataSets.Add(lOriginalData.DataSets.FindData("ID", 1004).Item(0))
                .Selected = True
            End With
            .Endpoints.Add(lEndPoint)

            .StartRun("modified")
            IO.File.WriteAllText("ClimateAssessmentFromScript.txt", pCat.ResultsGrid.ToString)
        End With
        Logger.Dbg("AllDone")
    End Sub

    Private Sub UpdateStatusLabel(ByVal aIteration As Integer) Handles pCat.StartIteration
        pCat.StartIterationMessage(aIteration)
    End Sub
End Module
