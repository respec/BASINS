Imports MapWinUtility
Imports atcData
Imports atcTimeseriesBinary

Module modOutputSummarize
    Sub OutputSummarize(ByVal aProjectFolder As String, ByVal aInputFilePath As String, ByVal aHuc12 As String)
        Dim lReportFilePath As String = IO.Path.Combine(aProjectFolder, "Scenarios\" & aHuc12 & "\TablesOut")

        Dim lOutputHruFileName As String = aInputFilePath & "\output.hru"
        If FileExists(lOutputHruFileName) Then
            Dim lOutputHru As New atcTimeseriesSWAT.atcTimeseriesSWAT
            With lOutputHru
                Dim lOutputFields As New atcData.atcDataAttributes
                lOutputFields.SetValue("FieldName", "AREAkm2;YLDt/ha")
                '.Open(pOutputFolder & "\tab.hru", lOutputFields)
                If .Open(lOutputHruFileName, lOutputFields) Then
                    Logger.Dbg("OutputHruTimserCount " & .DataSets.Count)
                    WriteDatasets(lReportFilePath & "\hru", .DataSets)
                End If
            End With
        Else
            Logger.Dbg("MissingHruOutput " & lOutputHruFileName)
        End If

        Dim lOutputRchFileName As String = aInputFilePath & "\output.rch"
        If FileExists(lOutputRchFileName) Then
            Dim lOutputRch As New atcTimeseriesSWAT.atcTimeseriesSWAT
            With lOutputRch
                If .Open(lOutputRchFileName) Then
                    Logger.Dbg("OutputRchTimserCount " & .DataSets.Count)
                    WriteDatasets(lReportFilePath & "\rch", .DataSets)
                End If
            End With
        Else
            Logger.Dbg("MissingRchOutput " & lOutputRchFileName)
        End If

        Dim lOutputSubFileName As String = aInputFilePath & "\output.sub"
        If FileExists(lOutputSubFileName) Then
            Dim lOutputSub As New atcTimeseriesSWAT.atcTimeseriesSWAT
            With lOutputSub
                If .Open(lOutputSubFileName) Then
                    Logger.Dbg("OutputSubTimserCount " & .DataSets.Count)
                    WriteDatasets(lReportFilePath & "\sub", .DataSets)
                End If
            End With
        Else
            Logger.Dbg("MissingSubOutput " & lOutputSubFileName)
        End If
    End Sub

    Private Sub WriteDatasets(ByVal aFileName As String, ByVal aDatasets As atcDataGroup)
        Dim lDataTarget As New atcDataSourceTimeseriesBinary ' atcDataSourceWDM
        Dim lFileName As String = aFileName & ".tsbin" 'lDataTarget.Filter.?) Then
        TryDelete(lFileName)
        If lDataTarget.Open(lFileName) Then
            lDataTarget.AddDatasets(aDatasets)
        End If
    End Sub
End Module
