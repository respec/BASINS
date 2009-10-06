Imports atcClimateAssessmentTool
Imports atcUtility
Imports MapWinUtility

''' <summary>
''' updates HSPF PERLND parameter tables with values extracted from an ATC standard spreadsheet of parameter values.
''' </summary>
''' <remarks></remarks>
Module modBatchCAT
    Sub main()
        If My.Application.CommandLineArgs.Count = 0 Then
            Logger.Msg("ERROR - No Command Line Argument Containing CAT Scenario Found", "BatchCAT Problem")
        Else
            Dim lCatSpecFile As String = My.Application.CommandLineArgs.Item(0)
            Dim lPath As String = IO.Path.GetDirectoryName(lCatSpecFile)
            My.Computer.FileSystem.CurrentDirectory = lPath
            Logger.StartToFile("logs\" & Format(Now, "yyyy-MM-dd") & "at" & Format(Now, "HH-mm") & "-BatchCAT.txt", , False)

            atcData.atcDataManager.Clear()
            Dim lCat As New atcClimateAssessmentTool.clsCat
            With lCat
                .XML = atcUtility.WholeFileString(lCatSpecFile)
                .RunModel = False
                .StartRun("")
            End With
        End If
    End Sub
End Module
