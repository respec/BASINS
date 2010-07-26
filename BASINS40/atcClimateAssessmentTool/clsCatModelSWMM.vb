Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class clsCatModelSWMM
    Implements clsCatModel

    Public Event BaseScenarioSet(ByVal aBaseScenario As String) Implements clsCatModel.BaseScenarioSet

    Private pBaseScenario As String = ""
    Private pBaseProject As atcSWMM.atcSWMMProject = Nothing

    Public Property BaseScenario() As String Implements clsCatModel.BaseScenario
        Get
            Return pBaseScenario
        End Get
        Set(ByVal newValue As String)
            OpenBaseScenario(newValue)
        End Set
    End Property

    ''' <summary>
    ''' Open data files referred to in this INP file
    ''' </summary>
    ''' <param name="aFilename">Full path of INP file</param>
    ''' <remarks></remarks>
    Friend Sub OpenBaseScenario(Optional ByVal aFilename As String = "")
        If Not aFilename Is Nothing And Not IO.File.Exists(aFilename) Then
            If IO.File.Exists(aFilename & ".inp") Then aFilename &= ".inp"
        End If

        If aFilename Is Nothing OrElse Not IO.File.Exists(aFilename) Then
            Dim cdlg As New Windows.Forms.OpenFileDialog
            cdlg.Title = "Open INP file containing base scenario"
            cdlg.Filter = "INP files|*.inp|All Files|*.*"
            If cdlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                aFilename = cdlg.FileName
            End If
        End If

        If IO.File.Exists(aFilename) Then
            Dim lInpFolder As String = PathNameOnly(aFilename)
            ChDriveDir(lInpFolder)
            pBaseScenario = aFilename
            RaiseEvent BaseScenarioSet(aFilename)
            pBaseProject = clsCat.OpenDataSource(aFilename)

            'Open the output file
            Dim lBinOutFilename As String = IO.Path.Combine(IO.Path.GetFileNameWithoutExtension(aFilename), ".out")
            clsCat.OpenDataSource(lBinOutFilename)
        End If
    End Sub

    Private Sub CreateModifiedINP(ByVal aNewScenarioName As String, ByVal aNewInpFilename As String)

        pBaseProject.Save(aNewInpFilename, atcDataSource.EnumExistAction.ExistReplace)

        'Dim lInpContents As String = WholeFileString(BaseScenario)
        'Dim lNewFilesBlock As String = ""
        'Dim lSaveLineEnd As String = ""
        'Dim lCurrentLine As String
        ''Dim lPathname As String
        'Dim lFilename As String


        ''Commented out code preserves path of original file, we are putting Modified files in same folder as modified UCI
        ''lPathname = PathNameOnly(lFilename)
        ''If lPathname.Length > 0 Then lPathname &= g_PathChar
        ''lFilename = lPathname & aNewScenarioName & "." & FilenameNoPath(lFilename)
        'lFilename = aNewScenarioName & ".inp"
        'lNewFilesBlock &= lInpContents
        'SaveFileString(aNewInpFilename, lInpContents.Substring(0, lStartFilesPos) & lNewFilesBlock & lInpContents.Substring(lEndFilesPos))
    End Sub

    ''' <summary>
    ''' Copy base INP and change scenario name within it
    ''' Change data to be modified in new INP
    ''' Run SWMM5 with the new INP
    ''' </summary>
    ''' <param name="aNewScenarioName"></param>
    ''' <param name="aModifiedData"></param>
    ''' <param name="aPreparedInput"></param>
    ''' <param name="aRunModel"></param>
    ''' <param name="aShowProgress"></param>
    ''' <param name="aKeepRunning"></param>
    ''' <returns>atcCollection of atcDataSource</returns>
    ''' <remarks></remarks>
    Public Function ScenarioRun(ByVal aNewScenarioName As String, _
                                ByVal aModifiedData As atcTimeseriesGroup, _
                                ByVal aPreparedInput As String, _
                                ByVal aRunModel As Boolean, _
                                ByVal aShowProgress As Boolean, _
                                ByVal aKeepRunning As Boolean) As atcCollection Implements clsCatModel.ScenarioRun

        Dim lModified As New atcCollection
        'Dim lCurrentTimeseries As atcTimeseries

        If aModifiedData Is Nothing Then
            aModifiedData = New atcTimeseriesGroup
        End If

        If IO.File.Exists(pBaseScenario) Then
            Dim lNewBaseFilename As String = AbsolutePath(pBaseScenario, CurDir)
            Dim lNewFolder As String = PathNameOnly(lNewBaseFilename) & g_PathChar
            lNewBaseFilename = lNewFolder & aNewScenarioName & "."
            Dim lNewINPfilename As String = ""

            'Get all external climate timeseries files, assuming they are named as *.DAT
            Select Case aNewScenarioName.ToLower
                Case "base"
                    lNewINPfilename = AbsolutePath(pBaseScenario, CurDir)
                    lModified.Add(IO.Path.GetFileName(lNewINPfilename).ToLower.Trim, lNewINPfilename.Trim)
                Case "modifyoriginal"
                    lNewINPfilename = AbsolutePath(pBaseScenario, CurDir)
                    lModified.Add(IO.Path.GetFileName(lNewINPfilename).ToLower.Trim, lNewINPfilename.Trim)

                    'TODO: needs work here to make sense: first add into, then clear???
                    'Dim lBinOutResults As atcTimeseriesSWMM5Output.atcDataSourceTimeseriesSWMM5Output = atcData.atcDataManager.DataSources(0) 'TODO: is it a good assumption that (0) is the results?

                    'For Each lDATfilename As String In lDATFilenames
                    '    lDATfilename = AbsolutePath(lDATfilename, CurDir).Trim()
                    '    For Each lCurrentTimeseries In aModifiedData
                    '        If Not lCurrentTimeseries Is Nothing _
                    '           AndAlso lCurrentTimeseries.Attributes.GetValue("History 1").ToString.ToLower.Equals("read from " & lDATfilename.ToLower) Then
                    '            lBinOutResults.AddDataSet(lCurrentTimeseries)
                    '            'Dim lmodifiedData As String = GetModifiedData(lType, aModifiedData)
                    '        End If
                    '    Next
                    '    lBinOutResults.DataSets.Clear()
                    'Next
                Case Else
                    lNewINPfilename = lNewBaseFilename & FilenameNoPath(pBaseScenario)
                    'Copy base INP, changing base to new scenario name within it
                    CreateModifiedINP(aNewScenarioName, lNewINPfilename)

                    'lDATfilename = AbsolutePath(lDATfilename, CurDir).Trim()
                    'If FilenameNoPath(lDATfilename).ToLower = FilenameNoPath(aPreparedInput).ToLower Then
                    '    lDATfilename = aPreparedInput
                    'End If
                    ''Copy each base DAT to new DAT only if simulation is to be rerun
                    'Dim lNewDATfilename As String = lNewFolder & aNewScenarioName & "." & IO.Path.GetFileName(lDATfilename)
                    'If aRunModel Then
                    '    FileCopy(lDATfilename, lNewDATfilename)
                    'End If
                    ''Dim lWDMResults As New atcWDM.atcDataSourceWDM
                    ''If Not lWDMResults.Open(lNewDATfilename) Then
                    ''    Logger.Msg("Could not open new scenario WDM file '" & lNewDATfilename & "'", MsgBoxStyle.Critical, "Could not run model")
                    ''    Return Nothing
                    ''End If

                    ''Key is base file name, value is modified file name
                    'lModified.Add(IO.Path.GetFileName(lDATfilename).ToLower.Trim, lNewDATfilename.Trim)

                    ' ''Update scenario name in new WDM
                    ''For Each lCurrentTimeseries In aModifiedData
                    ''    If Not lCurrentTimeseries Is Nothing _
                    ''       AndAlso lCurrentTimeseries.Attributes.GetValue("History 1").ToString.ToLower.Equals("read from " & lDATfilename.ToLower) Then
                    ''        lCurrentTimeseries.Attributes.SetValue("scenario", aNewScenarioName)
                    ''        lWDMResults.AddDataset(lCurrentTimeseries)
                    ''    End If
                    ''Next
                    ''For Each lCurrentTimeseries In lWDMResults.DataSets
                    ''    Dim lScenario As atcDefinedValue = lCurrentTimeseries.Attributes.GetDefinedValue("scenario")
                    ''    If lScenario.Value.ToLower = "base" Then
                    ''        lWDMResults.WriteAttribute(lCurrentTimeseries, lScenario, aNewScenarioName)
                    ''    End If
                    ''    If Not aModifiedData.Contains(lCurrentTimeseries) Then
                    ''        lCurrentTimeseries.ValuesNeedToBeRead = True
                    ''        lCurrentTimeseries.Attributes.DiscardCalculated()
                    ''    End If
                    ''Next
                    ''lWDMResults.DataSets.Clear()
            End Select

            Dim lRunExitCode As Integer = 0
            If aRunModel Then
                'Run scenario
                Dim lSWMMExeName As String = FindFile("Please locate epaswmm5.exe", g_PathChar & "Program Files\EPA SWMM 5.0\epaswmm5.exe")


                AppendFileString(lNewFolder & "SWMM5Error.Log", "Start log for " & lNewBaseFilename & vbCrLf)
                Dim lArgs As String = lNewINPfilename
                Logger.Dbg("Start " & lSWMMExeName & " with Arguments '" & lArgs & "'")
                Dim lSWMMProcess As New Diagnostics.Process
                With lSWMMProcess.StartInfo
                    .FileName = lSWMMExeName
                    .Arguments = lArgs
                    .CreateNoWindow = True
                    .UseShellExecute = False
                End With
                lSWMMProcess.Start()
                While Not lSWMMProcess.HasExited
                    If Not g_Running And Not aKeepRunning Then
                        lSWMMProcess.Kill()
                    End If
                    Windows.Forms.Application.DoEvents()
                    Threading.Thread.Sleep(50)
                End While
                lRunExitCode = lSWMMProcess.ExitCode
                Logger.Dbg("Model exit code " & lRunExitCode)
                If lRunExitCode <> 0 Then
                    Logger.Dbg("****************** Problem *********************")
                End If
            Else
                Logger.Dbg("Skipping running model")
            End If

            If g_Running Then
                If lRunExitCode <> 0 Then 'SWMM run failed, don't send any timeseries back to cat
                    lModified.Clear()
                Else
                    Dim lBinOutFilename As String = IO.Path.GetFileNameWithoutExtension(lNewINPfilename) & ".out"
                    lBinOutFilename = AbsolutePath(lBinOutFilename, CurDir)

                    If IO.File.Exists(lBinOutFilename) Then
                        'Dim lHBNResults As New atcHspfBinOut.atcTimeseriesFileHspfBinOut
                        'If lHBNResults.Open(lNewFilename) Then
                        lModified.Add(IO.Path.GetFileName(lBinOutFilename).ToLower.Trim, lBinOutFilename.Trim)
                        'Else
                        '    Logger.Dbg("Could not open output file '" & lBinOutFilename & "'")
                        'End If
                    Else
                        Logger.Dbg("Could not find binary .OUT file '" & lBinOutFilename & "'")
                    End If
                End If
            End If
        Else
            Logger.Msg("Could not find base INP file '" & pBaseScenario & "'" & vbCrLf & "Could not run model", "Scenario Run")
        End If
        Return lModified
    End Function

    Public Property XML() As String Implements clsCatModel.XML
        Get
            Dim lXML As String = ""
            lXML &= "<INP>" & vbCrLf
            lXML &= "  <FileName>" & pBaseScenario & "</FileName>" & vbCrLf
            lXML &= "</INP>" & vbCrLf
            Return lXML
        End Get
        Set(ByVal value As String)

        End Set
    End Property
End Class
