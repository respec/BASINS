Imports ScriptRunner

Module modScriptRunnerBatch
    Private g_Statistics As New atcTimeseriesStatistics.atcTimeseriesStatistics
    Sub main()
        Dim lCommandLine As String = Command()
        If lCommandLine.Length = 0 Then
            lCommandLine = "SantaClara"
        End If

        Try
            If g_Statistics.AvailableOperations.Count > 0 Then
                For Each lOperation As atcData.atcDefinedValue In g_Statistics.AvailableOperations
                    atcData.atcDataAttributes.AddDefinition(lOperation.Definition)
                Next
            End If
        Catch
        End Try

        HSPFOutputReports.ScriptMainStandAlone(lCommandLine)
    End Sub
End Module
