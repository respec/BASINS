Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class clsCatModelSWMM
    Implements clsCatModel

    Public SWMMProgramBase As String = "C:\Program Files\SWMM" & g_PathChar 'TODO: default to where SWMM is typically installed

    Public Event BaseScenarioSet(ByVal aBaseScenario As String) Implements clsCatModel.BaseScenarioSet

    Private pBaseScenario As String = ""
    Private pSWMMDatabaseName As String = ""
    Private pMetWDM As atcWDM.atcDataSourceWDM
    Private pBaseOutputFileName As String = Nothing

    Public Property BaseScenario() As String Implements clsCatModel.BaseScenario
        Get
            Return pBaseScenario
        End Get
        Set(ByVal newValue As String)
            OpenBaseScenario(newValue)
        End Set
    End Property

    Friend Sub OpenBaseScenario(Optional ByVal aFilename As String = "")
        If Not aFilename Is Nothing AndAlso Not IO.File.Exists(aFilename) Then
            If IO.File.Exists(aFilename & ".mdb") Then aFilename &= ".mdb"
        End If

        If aFilename Is Nothing OrElse Not IO.File.Exists(aFilename) Then
            Dim cdlg As New Windows.Forms.OpenFileDialog
            cdlg.Title = "Open SWMM file containing base scenario"
            cdlg.Filter = "SWMM mdb files|*.mdb|All Files|*.*"
            If cdlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                aFilename = cdlg.FileName
            End If
        End If

        If aFilename.ToLower <> pBaseScenario.ToLower AndAlso IO.File.Exists(aFilename) Then
            Dim lFolder As String = PathNameOnly(aFilename)
            ChDriveDir(lFolder)
            pBaseScenario = aFilename
            RaiseEvent BaseScenarioSet(aFilename)

            'Find and open met data WDM file
            Dim lWDMfilename As String = IO.Path.GetDirectoryName(aFilename) & "\met.wdm"
            If Not IO.File.Exists(lWDMfilename) Then
                lWDMfilename = IO.Path.GetDirectoryName(aFilename) & g_PathChar & "met\met.wdm"
            End If
OpenMetWDM:
            If IO.File.Exists(lWDMfilename) Then
                pMetWDM = clsCat.OpenDataSource(lWDMfilename)
            Else
                If Logger.Msg("Did not find '" & lWDMfilename & "'" & vbCrLf & "Browse for met data?", vbOKCancel, "SWMM Met Data Not Found") = MsgBoxResult.Ok Then
                    Dim lFileDialog As New Windows.Forms.OpenFileDialog
                    With lFileDialog
                        .Title = "Please locate met WDM for SWMM"
                        .Filter = "*.wdm|*.wdm"
                        If .ShowDialog = Windows.Forms.DialogResult.OK Then
                            lWDMfilename = .FileName
                            GoTo OpenMetWDM
                        End If
                    End With
                End If
            End If

            If pMetWDM IsNot Nothing Then
OpenOutput:
                'Find and open output from base run
                pBaseOutputFileName = "output.swmm"
                If FileExists(pBaseOutputFileName) Then
                    'Dim lOutputFile As New atcTimeseriesSWMMOutput.atcTimeseriesSWMMOutput
                    'With lOutputFile
                    '    If atcDataManager.OpenDataSource(lOutputFile, pBaseOutputFileName, Nothing) Then
                    '        Logger.Dbg("OutputTimserCount " & .DataSets.Count)
                    '    End If
                    'End With
                Else
                    If Logger.Msg("Run base scenario now?", vbYesNo, "SWMM output not found") = MsgBoxResult.Yes Then
                        pBaseOutputFileName = Nothing
                        ScenarioRun("Base", Nothing, "", True, True, False)
                        GoTo OpenOutput
                    Else
                        Logger.Dbg("MissingOutput " & pBaseOutputFileName)
                    End If
                End If
            End If
        End If
ALREADYSET:
    End Sub

    Public Function ScenarioRun(ByVal aNewScenarioName As String, _
                                ByVal aModifiedData As atcData.atcTimeseriesGroup, _
                                ByVal aPreparedInput As String, _
                                ByVal aRunModel As Boolean, _
                                ByVal aShowProgress As Boolean, _
                                ByVal aKeepRunning As Boolean) As atcUtility.atcCollection _
                                                        Implements clsCatModel.ScenarioRun
        Dim lSaveDir As String = CurDir()
        'Dim lSWMMInput As New atcSWMM.atcSWMMProject()

        'If aRunModel Then
        '    ChDir(lTxtInOutFolder)

        '    Dim lSWMMExe As String = IO.Path.Combine(IO.Path.GetDirectoryName(pBaseScenario), "SWMM.exe")
        '    If IO.File.Exists(lSWMMExe) Then
        '        Logger.Dbg("StartModel")
        '        LaunchProgram(lSWMMExe, lTxtInOutFolder)
        '        Logger.Dbg("DoneModelRun")
        '    Else
        '        Logger.Dbg("SWMM exe not found, skipping model run")
        '    End If
        'End If
        Dim lModified As New atcCollection

        '' Read written data for endpoints

        'Dim lOutputFileName As String = lTxtInOutFolder & "output.hru"
        'If FileExists(lOutputHruFileName) Then
        '    If pBaseOutputFileName Is Nothing Then 'running base
        '        pBaseOutputFileName = lOutputHruFileName
        '    Else
        '        lModified.Add(IO.Path.GetFileName(pBaseOutputFileName).ToLower.Trim, lOutputHruFileName.Trim)
        '    End If
        'Else
        '    Logger.Dbg("MissingHruOutput " & lOutputHruFileName)
        'End If

        ChDir(lSaveDir)
        Return lModified
    End Function

    Public Property XML() As String Implements clsCatModel.XML
        Get
            Dim lXML As String = ""
            lXML &= "<SWMM>" & vbCrLf
            lXML &= "  <FileName>" & pBaseScenario & "</FileName>" & vbCrLf
            lXML &= "</SWMM>" & vbCrLf
            Return lXML
        End Get
        Set(ByVal value As String)

        End Set
    End Property
End Class
