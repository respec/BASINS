Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Module modCAT
    <CLSCompliant(False)> _
    Friend g_MapWin As MapWindow.Interfaces.IMapWin
    Friend g_DataManager As atcDataManager

    Public Sub ScenarioRun(ByVal aCurrentWDMfilename As String, _
                                ByVal aNewScenarioName As String, _
                                ByVal aModifiedData As atcDataGroup, _
                                ByRef aWDMResults As atcDataSource, _
                                ByRef aHBNResults As atcDataSource)
        'Copy base UCI and change scenario name within it
        'Copy WDM
        'Change data to be modified in new WDM
        'Change scenario attribute in new WDM
        'Run WinHSPFlt with the new UCI

        Dim lNewFilename As String
        Dim lCurrentTimeseries As atcTimeseries
        aWDMResults = New atcWDM.atcDataSourceWDM
        aHBNResults = New atcHspfBinOut.atcTimeseriesFileHspfBinOut

        If FileExists(aCurrentWDMfilename) Then
            lNewFilename = AbsolutePath(aCurrentWDMfilename, CurDir)
            lNewFilename = PathNameOnly(lNewFilename) & "\" & aNewScenarioName & "."

            If aNewScenarioName.ToLower <> "base" Then
                'Copy base UCI and change scenario name within it
                ReplaceStringToFile(WholeFileString(FilenameSetExt(aCurrentWDMfilename, "uci")), "base.", aNewScenarioName & ".", lNewFilename & "uci")

                'Copy base WDM to new WDM
                FileCopy(aCurrentWDMfilename, lNewFilename & "wdm")
                If Not aWDMResults.Open(lNewFilename & "wdm") Then
                    Logger.Msg("Could not open new scenario WDM file '" & lNewFilename & "wdm'", MsgBoxStyle.Critical, "Could not run model")
                    Return
                End If

                'Update scenario name in new WDM
                For Each lCurrentTimeseries In aModifiedData
                    If Not lCurrentTimeseries Is Nothing Then
                        lCurrentTimeseries.Attributes.SetValue("scenario", aNewScenarioName)
                        aWDMResults.AddDataSet(lCurrentTimeseries)
                    End If
                Next
                For Each lCurrentTimeseries In aWDMResults.DataSets
                    If lCurrentTimeseries.Attributes.GetValue("scenario").ToLower = "base" Then
                        lCurrentTimeseries.EnsureValuesRead()
                        lCurrentTimeseries.Attributes.SetValue("scenario", aNewScenarioName)
                        aWDMResults.AddDataSet(lCurrentTimeseries) 'TODO: Would be nice to just update this attribute, not rewrite all data values
                        lCurrentTimeseries.ValuesNeedToBeRead = True
                        lCurrentTimeseries.Attributes.DiscardCalculated()
                    End If
                Next
            End If

            'Run scenario
            Dim lWinHspfLtExeName As String = FindFile("Please locate WinHspfLt.exe", "\BASINS\models\HSPF\bin\WinHspfLt.exe")
            Shell(lWinHspfLtExeName & " -1 -1 " & lNewFilename & "uci", AppWinStyle.NormalFocus, True)
            If FileExists(lNewFilename & ".hbn") Then
                aHBNResults.Open(lNewFilename & "hbn")
            End If
        Else
            Logger.Msg("Could not find base WDM file '" & aCurrentWDMfilename & "'" & vbCrLf & "Could not run model", "Scenario Run")
        End If
    End Sub
End Module
