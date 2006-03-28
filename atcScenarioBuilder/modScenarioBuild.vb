Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Module modScenarioBuild
  Public g_MapWin As MapWindow.Interfaces.IMapWin
  Public g_DataManager As atcDataManager

  Public Function ScenarioRun(ByVal aCurrentWDMfilename As String, _
                              ByVal aNewScenarioName As String, _
                              ByVal aModifiedData As atcDataGroup) As atcDataSource
    'Copy base UCI and change scenario name within it
    'Copy WDM
    'Change data to be modified in new WDM
    'Change scenario attribute in new WDM
    'Run WinHSPFlt with the new UCI

    Dim lNewFilename As String
    Dim lNewWDM As atcWDM.atcDataSourceWDM = Nothing

    If FileExists(aCurrentWDMfilename) Then
      lNewFilename = AbsolutePath(aCurrentWDMfilename, CurDir)
      lNewFilename = PathNameOnly(lNewFilename) & "\" & aNewScenarioName & "."

      If aNewScenarioName.ToLower <> "base" Then
        'Copy base UCI and change scenario name within it
        ReplaceStringToFile(WholeFileString(FilenameSetExt(aCurrentWDMfilename, "uci")), "base.", aNewScenarioName & ".", lNewFilename & "uci")

        'Copy base WDM to new WDM
        FileCopy(aCurrentWDMfilename, lNewFilename & "wdm")
        lNewWDM = New atcWDM.atcDataSourceWDM
        If Not lNewWDM.Open(lNewFilename & "wdm") Then
          MsgBox("Could not open new scenario WDM file '" & lNewFilename & "wdm'", MsgBoxStyle.Critical, "Could not run model")
          Return Nothing
        End If

        'Update scenario name in new WDM
        Dim lCurrentTimeseries As atcTimeseries
        For Each lCurrentTimeseries In aModifiedData
          If Not lCurrentTimeseries Is Nothing Then
            lCurrentTimeseries.Attributes.SetValue("scenario", aNewScenarioName)
            lNewWDM.AddDataSet(lCurrentTimeseries)
          End If
        Next
        For Each lCurrentTimeseries In lNewWDM.DataSets
          If lCurrentTimeseries.Attributes.GetValue("scenario").ToLower = "base" Then
            lCurrentTimeseries.EnsureValuesRead()
            lCurrentTimeseries.Attributes.SetValue("scenario", aNewScenarioName)
            lNewWDM.AddDataSet(lCurrentTimeseries) 'TODO: Would be nice to just update this attribute, not rewrite all data values
            lCurrentTimeseries.ValuesNeedToBeRead = True
            lCurrentTimeseries.Attributes.DiscardCalculated()
          End If
        Next
      End If

      'Run scenario
      Dim lWinHspfLtExeName As String = FindFile("Please locate WinHspfLt.exe", "\BASINS\models\HSPF\bin\WinHspfLt.exe")
      Shell(lWinHspfLtExeName & " -1 -1 " & lNewFilename & "uci", AppWinStyle.NormalFocus, True)

      Return lNewWDM

    Else
      Logger.Msg("Could not find base WDM file '" & aCurrentWDMfilename & "'" & vbCrLf & "Could not run model", "ScenarioBuild")
      Return Nothing
    End If
  End Function
End Module
