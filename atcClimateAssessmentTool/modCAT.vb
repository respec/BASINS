Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Module modCAT
    <CLSCompliant(False)> _
    Friend g_MapWin As MapWindow.Interfaces.IMapWin
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

    Private Sub CreateModifiedUCI(ByVal aBaseFilename As String, ByVal aNewScenarioName As String, ByVal aNewUciFilename As String)
        Dim lUciContents As String = WholeFileString(aBaseFilename)
        Dim lStartFilesPos As Integer = lUciContents.IndexOf(vbLf & "FILES") + 1
        Dim lEndFilesPos As Integer = lUciContents.IndexOf(vbLf & "END FILES", lStartFilesPos) + 1
        Dim lOriginalFilesBlock As String = lUciContents.Substring(lStartFilesPos, lEndFilesPos - lStartFilesPos)
        Dim lNewFilesBlock As String = ""
        Dim lSaveLineEnd As String = ""
        Dim lCurrentLine As String
        'Dim lPathname As String
        Dim lFilename As String

        For Each lCurrentLine In lOriginalFilesBlock.Split(vbLf)
            Select Case lCurrentLine.Length
                Case 0 'Ignore empty lines (last will be empty)
                Case Is < 16
                    lNewFilesBlock &= lCurrentLine & vbLf
                Case Else
                    lSaveLineEnd = Right(lCurrentLine, 1)
                    If lSaveLineEnd = vbCr Then
                        lCurrentLine = lCurrentLine.Substring(0, lCurrentLine.Length - 1)
                    Else
                        lSaveLineEnd = ""
                    End If
                    lFilename = lCurrentLine.Substring(16)
                    If lFilename.StartsWith("<") Then 'Not a file name
                        lNewFilesBlock &= lCurrentLine & lSaveLineEnd & vbLf
                    Else
                        'Commented out code preserves path of original file, we are putting Modified files in same folder as modified UCI
                        'lPathname = PathNameOnly(lFilename)
                        'If lPathname.Length > 0 Then lPathname &= "\"
                        'lFilename = lPathname & aNewScenarioName & "." & FilenameNoPath(lFilename)
                        lFilename = aNewScenarioName & "." & FilenameNoPath(lFilename)
                        lNewFilesBlock &= lCurrentLine.Substring(0, 16) & lFilename & lSaveLineEnd & vbLf
                    End If
            End Select
        Next

        SaveFileString(aNewUciFilename, lUciContents.Substring(0, lStartFilesPos) & lNewFilesBlock & lUciContents.Substring(lEndFilesPos))
    End Sub

    Public Function ScenarioRun(ByVal aBaseFilename As String, _
                           ByVal aNewScenarioName As String, _
                           ByVal aModifiedData As atcDataGroup, _
                           ByVal aPreparedInput As String, _
                           ByVal aRunModel As Boolean, _
                           ByVal aShowProgress As Boolean, _
                           ByVal aKeepRunning As Boolean) As atcCollection 'of atcDataSource
        'Copy base UCI and change scenario name within it
        'Copy WDM
        'Change data to be modified in new WDM
        'Change scenario attribute in new WDM
        'Run WinHSPFlt with the new UCI

        Dim lModified As New atcCollection
        Dim lCurrentTimeseries As atcTimeseries

        If aModifiedData Is Nothing Then aModifiedData = New atcDataGroup

        If FileExists(aBaseFilename) Then
            Dim lNewBaseFilename As String = AbsolutePath(aBaseFilename, CurDir)
            Dim lNewFolder As String = PathNameOnly(lNewBaseFilename) & "\"
            lNewBaseFilename = lNewFolder & aNewScenarioName & "."
            Dim lNewUCIfilename As String

            If aNewScenarioName.ToLower = "base" Then
                lNewUCIfilename = aBaseFilename
                Dim lWDMFilenames As ArrayList = UCIFilesBlockFilenames(WholeFileString(aBaseFilename), "WDM")
                For Each lWDMfilename As String In lWDMFilenames
                    lWDMfilename = AbsolutePath(lWDMfilename, CurDir)
                    lModified.Add(IO.Path.GetFileName(lWDMfilename).ToLower, lWDMfilename)
                Next
            Else
                Dim lWDMFilenames As ArrayList = UCIFilesBlockFilenames(WholeFileString(aBaseFilename), "WDM")
                lNewUCIfilename = lNewBaseFilename & FilenameNoPath(aBaseFilename)
                'Copy base UCI, changing base to new scenario name within it
                CreateModifiedUCI(aBaseFilename, aNewScenarioName, lNewUCIfilename)

                For Each lWDMfilename As String In lWDMFilenames
                    lWDMfilename = AbsolutePath(lWDMfilename, CurDir)
                    If FilenameNoPath(lWDMfilename).ToLower = FilenameNoPath(aPreparedInput).ToLower Then
                        lWDMfilename = aPreparedInput
                    End If
                    'Copy each base WDM to new WDM
                    Dim lNewWDMfilename As String = lNewFolder & aNewScenarioName & "." & IO.Path.GetFileName(lWDMfilename)
                    FileCopy(lWDMfilename, lNewWDMfilename)
                    Dim lWDMResults As New atcWDM.atcDataSourceWDM
                    If Not lWDMResults.Open(lNewWDMfilename) Then
                        Logger.Msg("Could not open new scenario WDM file '" & lNewWDMfilename & "'", MsgBoxStyle.Critical, "Could not run model")
                        Return Nothing
                    End If

                    'Key is base file name, value is modified file name
                    lModified.Add(IO.Path.GetFileName(lWDMfilename).ToLower, lNewWDMfilename)

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

            If aRunModel Then
                'Run scenario
                Dim lWinHspfLtExeName As String = FindFile("Please locate WinHspfLt.exe", "\BASINS\models\HSPF\bin\WinHspfLt.exe")

                Dim lPipeHandles As String = " -1 -1 "
                If aShowProgress Then lPipeHandles = " "

                'Shell(lWinHspfLtExeName & lPipeHandles & lNewBaseFilename & "uci", AppWinStyle.NormalFocus, True)

                AppendFileString(lNewFolder & "WinHSPFLtError.Log", "Start log for " & lNewBaseFilename & vbCrLf)
                Dim lArgs As String = lPipeHandles & lNewUCIfilename
                Logger.Dbg("Start " & lWinHspfLtExeName & " with Arguments '" & lArgs & "'")
                Dim newProc As Diagnostics.Process
                newProc = Diagnostics.Process.Start(lWinHspfLtExeName, lArgs)
                While Not newProc.HasExited
                    If Not g_running And Not aKeepRunning Then
                        newProc.Kill()
                    End If
                    Windows.Forms.Application.DoEvents()
                    Threading.Thread.Sleep(50)
                End While
                Logger.Dbg("Model exit code " & newProc.ExitCode)
                If newProc.ExitCode <> 0 Then
                    Logger.Dbg("****************** Problem *********************")
                End If
            Else
                Logger.Dbg("Skipping running model")
            End If
            If g_running Then
                For Each lBinOutFilename As String In UCIFilesBlockFilenames(WholeFileString(aBaseFilename), "BINO")
                    lBinOutFilename = AbsolutePath(lBinOutFilename, CurDir)
                    Dim lNewFilename As String
                    If aNewScenarioName.ToLower = "base" Then
                        lNewFilename = lBinOutFilename
                    Else
                        lNewFilename = PathNameOnly(lBinOutFilename) & "\" & aNewScenarioName & "." & IO.Path.GetFileName(lBinOutFilename)
                    End If
                    If IO.File.Exists(lNewFilename) Then
                        'Dim lHBNResults As New atcHspfBinOut.atcTimeseriesFileHspfBinOut
                        'If lHBNResults.Open(lNewFilename) Then
                        lModified.Add(IO.Path.GetFileName(lBinOutFilename).ToLower, lNewFilename)
                        'Else
                        '    Logger.Dbg("Could not open HBN file '" & lNewFilename & "'")
                        'End If
                    Else
                        Logger.Dbg("Could not find HBN file '" & lNewFilename & "'")
                    End If
                Next
            End If
        Else
            Logger.Msg("Could not find base UCI file '" & aBaseFilename & "'" & vbCrLf & "Could not run model", "Scenario Run")
        End If
        Return lModified
    End Function
End Module
