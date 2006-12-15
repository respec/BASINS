Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Module modCAT
    <CLSCompliant(False)> _
    Friend g_MapWin As MapWindow.Interfaces.IMapWin
    Friend g_DataManager As atcDataManager
    Friend g_running As Boolean = False

    ''' <summary>
    ''' Given the filename of a UCI file and a file type, return the file names, if any, of that type in the UCI
    ''' </summary>
    ''' <param name="aUCIfileContents">Full text of UCI file</param>
    ''' <param name="aFileType">WDM or MESSU or BINO</param>
    ''' <returns>ArrayList of file name(s) of the requested type appearing in the FILES block</returns>
    Public Function UCIFilesBlockFilenames(ByVal aUCIfileContents As String, ByVal aFileType As String) As ArrayList
        UCIFilesBlockFilenames = New ArrayList
        Dim lFilesBlock As String = StrFindBlock(aUCIfileContents, vbLf & "FILES", vbLf & "END FILES")
        Dim lNextPosition As Integer = lFilesBlock.IndexOf(vbLf & aFileType)
        Dim lEOL As Integer
        Dim lEOLchars() As Char = {ChrW(10), ChrW(13)}
        While lNextPosition >= 0
            lEOL = lFilesBlock.IndexOfAny(lEOLchars, lNextPosition + 1)
            UCIFilesBlockFilenames.Add(lFilesBlock.Substring(lNextPosition + 17, lEOL - lNextPosition - 17))
            lNextPosition = lFilesBlock.IndexOf(vbLf & aFileType, lEOL)
        End While
    End Function

    Public Function ScenarioRun(ByVal aBaseFilename As String, _
                           ByVal aNewScenarioName As String, _
                           ByVal aModifiedData As atcDataGroup, _
                           ByVal aShowProgress As Boolean) As atcCollection 'of atcDataSource
        'Copy base UCI and change scenario name within it
        'Copy WDM
        'Change data to be modified in new WDM
        'Change scenario attribute in new WDM
        'Run WinHSPFlt with the new UCI

        Dim lModified As New atcCollection

        Dim lCurrentTimeseries As atcTimeseries

        If FileExists(aBaseFilename) Then
            Dim lNewBaseFilename As String = AbsolutePath(aBaseFilename, CurDir)
            Dim lNewFolder As String = PathNameOnly(lNewBaseFilename) & "\"
            lNewBaseFilename = lNewFolder & aNewScenarioName & "."

            If aNewScenarioName.ToLower <> "base" Then
                'Copy base UCI, changing base to new scenario name within it
                ReplaceStringToFile(WholeFileString(aBaseFilename), "base.", aNewScenarioName & ".", lNewBaseFilename & "uci")
                For Each lWDMfilename As String In UCIFilesBlockFilenames(WholeFileString(aBaseFilename), "WDM")
                    lWDMfilename = AbsolutePath(lWDMfilename, CurDir)
                    'Copy each base WDM to new WDM
                    Dim lNewWDMfilename As String = lNewFolder & IO.Path.GetFileName(lWDMfilename).Replace("base.", aNewScenarioName & ".")
                    FileCopy(lWDMfilename, lNewWDMfilename)
                    Dim lWDMResults As New atcWDM.atcDataSourceWDM
                    If Not lWDMResults.Open(lNewWDMfilename) Then
                        Logger.Msg("Could not open new scenario WDM file '" & lNewWDMfilename & "'", MsgBoxStyle.Critical, "Could not run model")
                        Return Nothing
                    End If

                    'Key is base file name, value is modified file name
                    lModified.Add(lWDMfilename, lNewWDMfilename)

                    'Update scenario name in new WDM
                    For Each lCurrentTimeseries In aModifiedData
                        If Not lCurrentTimeseries Is Nothing _
                           AndAlso lCurrentTimeseries.Attributes.GetValue("History 1").ToString.ToLower.Equals("read from " & lWDMfilename.ToLower) Then
                            lCurrentTimeseries.Attributes.SetValue("scenario", aNewScenarioName)
                            lWDMResults.AddDataset(lCurrentTimeseries)
                        End If
                    Next
                    For Each lCurrentTimeseries In lWDMResults.DataSets
                        Dim lScenario As atcDefinedValue = lCurrentTimeseries.Attributes.GetDefinedValue("scenario")
                        If lScenario.Value.ToLower = "base" Then
                            lWDMResults.WriteAttribute(lCurrentTimeseries, lScenario, aNewScenarioName)
                        End If
                        If Not aModifiedData.Contains(lCurrentTimeseries) Then
                            lCurrentTimeseries.ValuesNeedToBeRead = True
                            lCurrentTimeseries.Attributes.DiscardCalculated()
                        End If
                    Next
                    lWDMResults.DataSets.Clear()
                Next
            End If

            'Run scenario
            Dim lWinHspfLtExeName As String = FindFile("Please locate WinHspfLt.exe", "\BASINS\models\HSPF\bin\WinHspfLt.exe")

            Dim lPipeHandles As String = " -1 -1 "
            If aShowProgress Then lPipeHandles = " "

            'Shell(lWinHspfLtExeName & lPipeHandles & lNewBaseFilename & "uci", AppWinStyle.NormalFocus, True)
            Dim newProc As Diagnostics.Process
            newProc = Diagnostics.Process.Start(lWinHspfLtExeName, lPipeHandles & lNewBaseFilename & "uci")
            While Not newProc.HasExited
                If Not g_running Then newProc.Kill()
                Windows.Forms.Application.DoEvents()
                Threading.Thread.Sleep(50)
            End While
            Logger.Dbg("Model exit code " & newProc.ExitCode)

            For Each lBinOutFilename As String In UCIFilesBlockFilenames(WholeFileString(aBaseFilename), "BINO")
                lBinOutFilename = AbsolutePath(lBinOutFilename, CurDir)
                Dim lNewFilename As String = lBinOutFilename.Replace("base.", aNewScenarioName & ".")
                Dim lHBNResults As New atcHspfBinOut.atcTimeseriesFileHspfBinOut
                lHBNResults.Open(lNewFilename)
                lModified.Add(lBinOutFilename, lNewFilename)
            Next
        Else
            Logger.Msg("Could not find base UCI file '" & aBaseFilename & "'" & vbCrLf & "Could not run model", "Scenario Run")
        End If
        Return lModified
    End Function
End Module
